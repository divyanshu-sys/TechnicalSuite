using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.CountryDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.CountryDtos;
using Ts.Dto.DataTableDtos.CountryDataTableDtos;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.Service.DataServices
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<CountryService> logger;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CountryService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<CountryDto>> CreateAsync(CreateCountryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<CountryDto>();
            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<Country>(modelDto);
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.CountryRepo.Create(entity);
            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                    responseResult.Data = mapper.Map<CountryDto>(entity);
                else
                    responseResult.ErrorMessage.Add("Create Country failed.");
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error creating country: {ErrorMessage}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.CountryRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Country you want to delete was not found.");
            else
            {
                try
                {
                    unitOfWork.CountryRepo.Delete(entity);
                    var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                    if (rowsChanged > 0)
                        responseResult.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    string msg = ex.GetFriendlySqlExceptionMessage();
                    responseResult.ErrorMessage.Add(msg);
                    logger.LogError(ex, "Error deleting country: {ErrorMessage}", msg);
                }
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<CountryDto>> GetAllAsync(CountryDataTableRequestDto modelDto)
        {
            var model = mapper.Map<CountryDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<CountryDto>
            {
                AaData = mapper.Map<List<CountryDto>>(await unitOfWork.CountryRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.CountryRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.CountryRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            var entities = await unitOfWork.CountryRepo.GetAllAsync().ConfigureAwait(false);
            return mapper.Map<List<CountryDto>>(entities);
        }

        public async Task<ResponseMessageDto<CountryDto>> GetAsync(int id)
        {
            var responseResult = new ResponseMessageDto<CountryDto>();

            var country = await unitOfWork.CountryRepo.GetAsync(id).ConfigureAwait(false);
            if (country == null)
                responseResult.ErrorMessage.Add("Country not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<CountryDto>(country);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int countryId, UpdateCountryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.CountryRepo.GetAsync(countryId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Country does not exist to update.");
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
                logger.LogError(ex, "Error updating country: {ErrorMessage}", msg);
            }

            return responseResult;
        }
    }
}
