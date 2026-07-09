using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.StateDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.DataTableDtos.StateDataTableDtos;
using Ts.Dto.StateDtos;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.Service.DataServices
{
    public class StateService : IStateService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<StateService> logger;

        public StateService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StateService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<StateDto>> CreateAsync(CreateStateDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<StateDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<State>(modelDto);
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.StateRepo.Create(entity);
            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                    responseResult.Data = mapper.Map<StateDto>(entity);
                else
                    responseResult.ErrorMessage.Add("Create State failed.");
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error creating State: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StateRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("State you want to delete was not found.");
            else
            {
                try
                {
                    unitOfWork.StateRepo.Delete(entity);
                    var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                    if (rowsChanged > 0)
                        responseResult.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    string msg = ex.GetFriendlySqlExceptionMessage();
                    responseResult.ErrorMessage.Add(msg);
                    logger.LogError(ex, "Error deleting State: {Message}", msg);
                }
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<GetStateDataTableDto>> GetAllAsync(StateDataTableRequestDto modelDto)
        {
            var model = mapper.Map<StateDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<GetStateDataTableDto>
            {
                AaData = mapper.Map<List<GetStateDataTableDto>>(await unitOfWork.StateRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.StateRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.StateRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<IEnumerable<StateDto>> GetAllByCountryIdAsync(int countryId)
        {
            var enities = await unitOfWork.StateRepo.GetAllByCountryIdAsync(countryId).ConfigureAwait(false);
            return mapper.Map<List<StateDto>>(enities);
        }

        public async Task<ResponseMessageDto<StateDto>> GetAsync(int id)
        {
            var responseResult = new ResponseMessageDto<StateDto>();

            var state = await unitOfWork.StateRepo.GetAsync(id).ConfigureAwait(false);
            if (state == null)
                responseResult.ErrorMessage.Add("State not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<StateDto>(state);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int stateId, UpdateStateDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StateRepo.GetAsync(stateId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("State does not exist to update.");
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
                logger.LogError(ex, "Error updating State: {Message}", msg);
            }

            return responseResult;
        }
    }
}
