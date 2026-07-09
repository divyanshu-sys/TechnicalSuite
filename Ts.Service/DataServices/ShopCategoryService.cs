using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.ShopCategoryDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.DataTableDtos.ShopCategoryDataTableDtos;
using Ts.Dto.ShopCategoryDtos;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.Service.DataServices
{
    public class ShopCategoryService : IShopCategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;
        private readonly ILogger<ShopCategoryService> logger;
        private const string EntityCacheKey = "ShopCategory";

        public ShopCategoryService(IUnitOfWork unitOfWork, IMapper mapper,
            IDistributedCache cache, ILogger<ShopCategoryService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<ShopCategoryDto>> CreateAsync(CreateShopCategoryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<ShopCategoryDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<ShopCategory>(modelDto);
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.ShopCategoryRepo.Create(entity);
            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                {
                    responseResult.Data = mapper.Map<ShopCategoryDto>(entity);
                    await cache.RemoveAsync(EntityCacheKey).ConfigureAwait(false);
                }
                else
                    responseResult.ErrorMessage.Add("Create Shop Category failed.");
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error creating Shop Category: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<ShopCategoryDto>> GetAllAsync(ShopCategoryDataTableRequestDto modelDto)
        {
            var model = mapper.Map<ShopCategoryDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<ShopCategoryDto>
            {
                AaData = mapper.Map<List<ShopCategoryDto>>(await unitOfWork.ShopCategoryRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.ShopCategoryRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.ShopCategoryRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<IEnumerable<ShopCategoryDto>> GetAllAsync()
        {
            var cachedEntity = await cache.GetStringAsync(EntityCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<ShopCategoryDto>>(cachedEntity);

            var entities = await unitOfWork.ShopCategoryRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<ShopCategoryDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(EntityCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);

            return modelDto;
        }

        public async Task<ResponseMessageDto<ShopCategoryDto>> GetAsync(int id)
        {
            var responseResult = new ResponseMessageDto<ShopCategoryDto>();

            var country = await unitOfWork.ShopCategoryRepo.GetAsync(id).ConfigureAwait(false);
            if (country == null)
                responseResult.ErrorMessage.Add("Shop Category not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<ShopCategoryDto>(country);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int ShopCategoryId, UpdateShopCategoryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));
            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ShopCategoryRepo.GetAsync(ShopCategoryId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Shop Category does not exist to update.");
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
                logger.LogError(ex, "Error updating Shop Category: {Message}", msg);
            }

            return responseResult;
        }
    }
}
