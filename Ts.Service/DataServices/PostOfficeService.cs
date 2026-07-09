using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.PostOfficeDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.DataTableDtos.PostOfficeDataTableDtos;
using Ts.Dto.PostOfficeDtos;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.Service.DataServices
{
    public class PostOfficeService : IPostOfficeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<PostOfficeService> logger;

        public PostOfficeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostOfficeService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<PostOfficeDto>> CreateAsync(CreatePostOfficeDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<PostOfficeDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<PostOffice>(modelDto);
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.PostOfficeRepo.Create(entity);
            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                    responseResult.Data = mapper.Map<PostOfficeDto>(entity);
                else
                    responseResult.ErrorMessage.Add("Create PostOffice failed.");
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error creating PostOffice: {ErrorMessage}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostOfficeRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("PostOffice you want to delete was not found.");
            else
            {
                try
                {
                    unitOfWork.PostOfficeRepo.Delete(entity);
                    var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                    if (rowsChanged > 0)
                        responseResult.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    string msg = ex.GetFriendlySqlExceptionMessage();
                    responseResult.ErrorMessage.Add(msg);
                    logger.LogError(ex, "Error deleting PostOffice: {ErrorMessage}", msg);
                }
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<GetPostOfficeDataTableDto>> GetAllAsync(PostOfficeDataTableRequestDto modelDto)
        {
            var model = mapper.Map<PostOfficeDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<GetPostOfficeDataTableDto>
            {
                AaData = mapper.Map<List<GetPostOfficeDataTableDto>>(await unitOfWork.PostOfficeRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.PostOfficeRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.PostOfficeRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<IEnumerable<PostOfficeDto>> GetAllByDistrictIdAsync(int districtId)
        {
            var entities = await unitOfWork.PostOfficeRepo.GetAllByDistrictIdAsync(districtId).ConfigureAwait(false);
            return mapper.Map<List<PostOfficeDto>>(entities);
        }

        public async Task<ResponseMessageDto<PostOfficeDto>> GetAsync(int id)
        {
            var responseResult = new ResponseMessageDto<PostOfficeDto>();

            var state = await unitOfWork.PostOfficeRepo.GetAsync(id).ConfigureAwait(false);
            if (state == null)
                responseResult.ErrorMessage.Add("PostOffice not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<PostOfficeDto>(state);

            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdatePostOfficeDto>> GetForEditAsync(int id)
        {
            var responseResult = new ResponseMessageDto<GetUpdatePostOfficeDto>();

            var state = await unitOfWork.PostOfficeRepo.GetAsync(id).ConfigureAwait(false);
            if (state == null)
                responseResult.ErrorMessage.Add("PostOffice not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<GetUpdatePostOfficeDto>(state);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int postOfficeId, UpdatePostOfficeDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostOfficeRepo.GetAsync(postOfficeId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("PostOffice does not exist to update.");
                return responseResult;
            }

            modelDto.HtmlEncodeObject();
            mapper.Map(modelDto, entity);
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                    responseResult.Data = true;
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error updating PostOffice: {ErrorMessage}", msg);
            }

            return responseResult;
        }
    }
}
