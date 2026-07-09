using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.CategoryDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.CategoryDtos;
using Ts.Dto.DataTableDtos.CategoryDataTableDtos;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.Service.DataServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;
        private readonly ILogger<CategoryService> logger;
        private const string EntityCacheKey = "Category";

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper,
            IDistributedCache cache, ILogger<CategoryService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<CategoryDto>> CreateAsync(CreateCategoryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<CategoryDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<Category>(modelDto);
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.CategoryRepo.Create(entity);
            try
            {
                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                if (rowsChanged > 0)
                {
                    responseResult.Data = mapper.Map<CategoryDto>(entity);
                    await cache.RemoveAsync(EntityCacheKey).ConfigureAwait(false);
                }
                else
                    responseResult.ErrorMessage.Add("Create Category failed.");
            }
            catch (DbUpdateException ex)
            {
                string msg = ex.GetFriendlySqlExceptionMessage();
                responseResult.ErrorMessage.Add(msg);
                logger.LogError(ex, "Error creating category: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<CategoryDto>> GetAllAsync(CategoryDataTableRequestDto modelDto)
        {
            var model = mapper.Map<CategoryDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<CategoryDto>
            {
                AaData = mapper.Map<List<CategoryDto>>(await unitOfWork.CategoryRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.CategoryRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.CategoryRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var cachedEntity = await cache.GetStringAsync(EntityCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(cachedEntity);

            var entities = await unitOfWork.CategoryRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<CategoryDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(EntityCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);

            return modelDto;
        }

        public async Task<ResponseMessageDto<CategoryDto>> GetAsync(int id)
        {
            var responseResult = new ResponseMessageDto<CategoryDto>();

            var country = await unitOfWork.CategoryRepo.GetAsync(id).ConfigureAwait(false);
            if (country == null)
                responseResult.ErrorMessage.Add("Category not found by supplied Id.");
            else
                responseResult.Data = mapper.Map<CategoryDto>(country);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int categoryId, UpdateCategoryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.CategoryRepo.GetAsync(categoryId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Category does not exist to update.");
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
                logger.LogError(ex, "Error updating category: {Message}", msg);
            }

            return responseResult;
        }
    }
}
