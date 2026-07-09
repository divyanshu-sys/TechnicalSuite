using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Application.HelperExtensions;
using Ts.Common.AppInterfaces;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
using Ts.ShopIn.Domain.DataTableModels.ProductDataTables;
using Ts.ShopIn.Domain.HelperModels;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
using Ts.ShopIn.Dto.ProductDtos;
using Ts.ShopIn.Dto.ProductImageDtos;
using Ts.ShopIn.Service.DataInterfaces;
using Ts.ShopIn.Service.HttpClientServices.ClientInterfaces;

namespace Ts.ShopIn.Service.DataServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFileClient fileClient;
        private readonly IFileValidationService fileValidationService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IConfiguration config;
        private readonly IShopCategoryService shopCategoryService;
        private readonly IDropDownService dropDownService;
        private readonly ILogger<ProductService> logger;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper,
            IFileClient fileClient, IFileValidationService fileValidationService,
            IApplicationUserService applicationUserService, IConfiguration config,
            IShopCategoryService shopCategoryService, IDropDownService dropDownService, ILogger<ProductService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.fileClient = fileClient;
            this.fileValidationService = fileValidationService;
            this.applicationUserService = applicationUserService;
            this.config = config;
            this.shopCategoryService = shopCategoryService;
            this.dropDownService = dropDownService;
            this.logger = logger;
        }

        public async Task<ResponseMessageDto<ProductDto>> CreateAsync(CreateProductDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<ProductDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<Product>(modelDto);
            entity.ProductWorkerId = userId;
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            unitOfWork.ProductRepo.Create(entity);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            await unitOfWork.CommitAsync().ConfigureAwait(false);

            responseResult.Data = mapper.Map<ProductDto>(entity);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Product you want to delete was not found.");
            else if (!isAdmin && entity.ProductWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to delete this product.");
            else
            {
                var imageNames = new List<string>();

                var productImageHelperModels = JsonConvert.DeserializeObject<IEnumerable<ProductImageHelperModel>>(entity.ProductImage?.ImageNames ?? "[]");
                imageNames.AddRange(from productImageHelperModel in productImageHelperModels
                                    select productImageHelperModel.Name);

                unitOfWork.ProductRepo.Delete(entity);

                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                var fileResponse = await fileClient.DeleteProductImagesAsync(imageNames).ConfigureAwait(false);
                if (fileResponse.ErrorMessage.Count > 0)
                {
                    if (rowsChanged > 0)
                        responseResult.ErrorMessage.Add("Product is deleted but unable to delete its all images!");

                    foreach (var error in fileResponse.ErrorMessage)
                    {
                        responseResult.ErrorMessage.Add(error);
                    }
                    return responseResult;
                }

                if (rowsChanged > 0)
                    responseResult.Data = true;
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<GetProductDataTableDto>> GetAllAsync(ProductDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null)
        {
            var model = mapper.Map<ProductDataTableRequest>(modelDto);

            if (isAdmin.HasValue && !isAdmin.Value)
                model.ProductWorkerId = userId;
            var responseResult = new DataTableResponseDto<GetProductDataTableDto>
            {
                AaData = mapper.Map<List<GetProductDataTableDto>>(await unitOfWork.ProductRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.ProductRepo.GetTotalCountAsync().ConfigureAwait(false)
            };

            var userIds = responseResult.AaData.SelectMany(x => new List<string> { x.ProductWorkerId, x.PublishedById }).Distinct();
            var applicationUsers = await applicationUserService.GetUsersByUserIdsAsync(userIds).ConfigureAwait(false);

            foreach (var product in responseResult.AaData)
            {
                product.ProductWorker = applicationUsers.SingleOrDefault(x => x.Id == product.ProductWorkerId);
                if (!string.IsNullOrEmpty(product.PublishedById))
                    product.PublishedBy = applicationUsers.SingleOrDefault(x => x.Id == product.PublishedById);
            }

            if (!string.IsNullOrEmpty(modelDto.Search?.Value) || !string.IsNullOrEmpty(modelDto.ProductWorkerId))
                responseResult.RecordsFiltered = await unitOfWork.ProductRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdateProductDto>> GetForEditAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<GetUpdateProductDto>();

            var entity = await unitOfWork.ProductRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Product not found by supplied Id.");
            else if (!isAdmin && entity.ProductWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to update this product.");
            else
                responseResult.Data = mapper.Map<GetUpdateProductDto>(entity);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> PublishProductAsync(int productId, PublishProductDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductRepo.GetAsync(productId).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Product does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this product.");
                return responseResult;
            }

            if (modelDto.IsRepublish && entity.PublishedById != null || !entity.IsPublished && entity.PublishedById == null)
            {
                entity.IsPublished = true;
                entity.PublishedOn = DateTime.UtcNow;
                entity.PublishedById = userId;
            }
            else
            {
                entity.IsPublished = !entity.IsPublished;
            }

            modelDto.HtmlEncodeObject();
            mapper.Map(modelDto, entity);
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int productId, UpdateProductDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductRepo.GetAsync(productId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Product does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this product.");
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
                logger.LogError(ex, "Error updating product: {ErrorMessage}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductWorkerAsync(int productId, UpdateProductWorkerDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductRepo.GetAsync(productId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Product does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this product.");
                return responseResult;
            }

            modelDto.HtmlEncodeObject();
            mapper.Map(modelDto, entity);
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int productId, UpdateProductMainImageDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductRepo.GetAsync(productId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Product does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this product.");
                return responseResult;
            }

            modelDto.HtmlEncodeObject();
            mapper.Map(modelDto, entity);
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductImageAsync(int productId, UpdateProductImageDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            if (!await fileValidationService.ValidateImageFileSizeAsync(modelDto.ImageFile).ConfigureAwait(false))
            {
                responseResult.ErrorMessage.Add(fileValidationService.ImageFileSizeInvalidMsg);
                return responseResult;
            }
            if (!await fileValidationService.ValidateImageFileExtensionAsync(modelDto.ImageFile).ConfigureAwait(false))
            {
                responseResult.ErrorMessage.Add(fileValidationService.ImageFileExtensionInValidMsg);
                return responseResult;
            }

            var entity = await unitOfWork.ProductRepo.GetAsync(productId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Product does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this product.");
                return responseResult;
            }

            var productImageHelperModels = JsonConvert.DeserializeObject<List<ProductImageHelperModel>>(entity.ProductImage?.ImageNames ?? "[]");

            var fileResponse = await fileClient.UploadProductImageAsync(modelDto.ImageFile).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            productImageHelperModels.Add(new ProductImageHelperModel
            {
                Name = fileResponse.Data
            });
            var imageNames = JsonConvert.SerializeObject(productImageHelperModels);

            modelDto.HtmlEncodeObject();
            if (entity.ProductImage != null)
            {
                entity.ProductImage.ImageNames = imageNames;
                entity.ProductImage.IpAddress = modelDto.IpAddress;
                entity.ProductImage.UpdatedById = userId;
                entity.ProductImage.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ProductImage = new()
                {
                    Id = entity.Id,
                    ImageNames = imageNames,
                    IpAddress = modelDto.IpAddress,
                    CreatedById = userId,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true
                };
            }

            entity.IpAddress = modelDto.IpAddress;
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductImageAsync(int id, string imageName, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Product not found for the deletion of image.");
                return responseResult;
            }
            if (!isAdmin && entity.ProductWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this product.");
                return responseResult;
            }

            var productImageHelperModels = JsonConvert.DeserializeObject<List<ProductImageHelperModel>>(entity.ProductImage?.ImageNames ?? "[]");
            var productImageHelperModel = productImageHelperModels.SingleOrDefault(x => x.Name == imageName);
            if (productImageHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Image not found to delete.");
                return responseResult;
            }
            productImageHelperModels.Remove(productImageHelperModel);

            var fileResponse = await fileClient.DeleteProductImagesAsync([imageName]).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            if (productImageHelperModels.Count == 0)
                entity.ProductImage = null;
            else
            {
                var imageNames = JsonConvert.SerializeObject(productImageHelperModels);

                entity.ProductImage.ImageNames = imageNames;
                entity.UpdatedById = userId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<IEnumerable<GetProductForListViewDto>> GetForListViewAsync(ProductDataTableForViewRequestDto modelDto)
        {
            return await CommonProductService.GetForListViewAsync(unitOfWork, mapper, shopCategoryService, config, modelDto, dropDownService).ConfigureAwait(false);
        }

        public async Task<IEnumerable<DropdownItemDto>> GetAllForDropDownAsync()
        {
            var dtos = await unitOfWork.ProductRepo.GetAllAsync().ConfigureAwait(false);

            var modelDto = dtos
                        .Select(x => new DropdownItemDto
                        {
                            Key = x.Id,
                            Value = x.Title
                        });

            return modelDto;
        }
    }
}
