using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.SubCategoryDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.DataTableDtos.SubCategoryDataTableDtos;
using Ts.Dto.SubCategoryDtos;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.Service.DataServices
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;
        private readonly ILogger<SubCategoryService> logger;
        private const string EntityCacheKey = "SubCategory";

        public SubCategoryService(IUnitOfWork unitOfWork, IMapper mapper,
            IDistributedCache cache, ILogger<SubCategoryService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<SubCategoryDto>> CreateAsync(CreateSubCategoryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<SubCategoryDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<SubCategory>(modelDto);
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.SubCategoryRepo.Create(entity);
            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                {
                    responseResult.Data = mapper.Map<SubCategoryDto>(entity);
                    await cache.RemoveAsync(EntityCacheKey).ConfigureAwait(false);
                }
                else
                    responseResult.ErrorMessage.Add("Create Sub Category failed.");
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error creating Sub Category: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<SubCategoryDto>> GetAllAsync(SubCategoryDataTableRequestDto modelDto)
        {
            var model = mapper.Map<SubCategoryDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<SubCategoryDto>
            {
                AaData = mapper.Map<List<SubCategoryDto>>(await unitOfWork.SubCategoryRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.SubCategoryRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.SubCategoryRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<IEnumerable<SubCategoryDto>> GetAllAsync()
        {
            var cachedEntity = await cache.GetStringAsync(EntityCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<SubCategoryDto>>(cachedEntity);

            var entities = await unitOfWork.SubCategoryRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<SubCategoryDto>>(entities);

            await cache.SetStringAsync(EntityCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);

            return modelDto;
        }

        public async Task<ResponseMessageDto<SubCategoryDto>> GetAsync(int id)
        {
            var responseResult = new ResponseMessageDto<SubCategoryDto>();

            var country = await unitOfWork.SubCategoryRepo.GetAsync(id).ConfigureAwait(false);
            if (country == null)
                responseResult.ErrorMessage.Add("Sub Category not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<SubCategoryDto>(country);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int subCategoryId, UpdateSubCategoryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));
            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.SubCategoryRepo.GetAsync(subCategoryId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Sub Category does not exist to update.");
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
                {
                    responseResult.Data = true;
                    await cache.RemoveAsync(EntityCacheKey).ConfigureAwait(false);
                }
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error updating Sub Category: {Message}", msg);
            }

            return responseResult;
        }
    }
}
