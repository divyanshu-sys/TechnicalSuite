using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.DistrictDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.DataTableDtos.DistrictDataTableDtos;
using Ts.Dto.DistrictDtos;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.Service.DataServices
{
    public class DistrictService : IDistrictService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<DistrictService> logger;

        public DistrictService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DistrictService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<DistrictDto>> CreateAsync(CreateDistrictDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<DistrictDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<District>(modelDto);
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.DistrictRepo.Create(entity);
            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                    responseResult.Data = mapper.Map<DistrictDto>(entity);
                else
                    responseResult.ErrorMessage.Add("Create District failed.");
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error creating district: {ErrorMessage}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.DistrictRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("District you want to delete was not found.");
            else
            {
                try
                {
                    unitOfWork.DistrictRepo.Delete(entity);
                    var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                    if (rowsChanged > 0)
                        responseResult.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    string msg = ex.GetFriendlySqlExceptionMessage();
                    responseResult.ErrorMessage.Add(msg);
                    logger.LogError(ex, "Error deleting district: {ErrorMessage}", msg);
                }
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<GetDistrictDataTableDto>> GetAllAsync(DistrictDataTableRequestDto modelDto)
        {
            var model = mapper.Map<DistrictDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<GetDistrictDataTableDto>
            {
                AaData = mapper.Map<List<GetDistrictDataTableDto>>(await unitOfWork.DistrictRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.DistrictRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.DistrictRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<IEnumerable<DistrictDto>> GetAllByStateIdAsync(int stateId)
        {
            var entities = await unitOfWork.DistrictRepo.GetAllByStateIdAsync(stateId).ConfigureAwait(false);
            return mapper.Map<List<DistrictDto>>(entities);
        }

        public async Task<ResponseMessageDto<DistrictDto>> GetAsync(int id)
        {
            var responseResult = new ResponseMessageDto<DistrictDto>();

            var state = await unitOfWork.DistrictRepo.GetAsync(id).ConfigureAwait(false);
            if (state == null)
                responseResult.ErrorMessage.Add("District not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<DistrictDto>(state);

            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdateDistrictDto>> GetForEditAsync(int id)
        {
            var responseResult = new ResponseMessageDto<GetUpdateDistrictDto>();

            var state = await unitOfWork.DistrictRepo.GetAsync(id).ConfigureAwait(false);
            if (state == null)
                responseResult.ErrorMessage.Add("District not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<GetUpdateDistrictDto>(state);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int districtId, UpdateDistrictDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.DistrictRepo.GetAsync(districtId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("District does not exist to update.");
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
                logger.LogError(ex, "Error updating district: {ErrorMessage}", msg);
            }

            return responseResult;
        }
    }
}
