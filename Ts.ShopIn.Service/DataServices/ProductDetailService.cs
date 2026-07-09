using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using Ts.Application.HelperExtensions;
using Ts.Common.AppInterfaces;
using Ts.Common.Constant.AppConstants;
using Ts.Common.Constant.SiteConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
using Ts.ShopIn.Domain.DataTableModels.ProductDetailDataTables;
using Ts.ShopIn.Domain.HelperModels;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;
using Ts.ShopIn.Dto.ProductDetailImageDtos;
using Ts.ShopIn.Service.DataInterfaces;
using Ts.ShopIn.Service.Helpers;
using Ts.ShopIn.Service.HttpClientServices.ClientInterfaces;
namespace Ts.ShopIn.Service.DataServices
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ProductDetailService> logger;
        private readonly IFileClient fileClient;
        private readonly IFileValidationService fileValidationService;
        private readonly IShopCategoryService shopCategoryService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IDropDownService dropDownService;

        public ProductDetailService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductDetailService> logger,
            IFileClient fileClient, IFileValidationService fileValidationService,
            IShopCategoryService shopCategoryService,
            IApplicationUserService applicationUserService, IConfiguration config,
            IWebHostEnvironment webHostEnvironment, IDropDownService dropDownService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.fileClient = fileClient;
            this.fileValidationService = fileValidationService;
            this.shopCategoryService = shopCategoryService;
            this.applicationUserService = applicationUserService;
            this.config = config;
            this.webHostEnvironment = webHostEnvironment;
            this.dropDownService = dropDownService;
        }

        public async Task<ResponseMessageDto<ProductDetailDto>> CreateAsync(CreateProductDetailDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<ProductDetailDto>();
            if (modelDto.Price > modelDto.Mrp)
            {
                responseResult.ErrorMessage.Add("Price should not be greater than MRP.");
                return responseResult;
            }

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<ProductDetail>(modelDto);
            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            if (CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies).Contains(entity.DeliveryPolicyId))
                entity.Stock = 0;

            entity.Mrp = CommonProductDetailService.RoundingToDecimal(entity.Mrp, CurrencyTypeConstant.INR);
            entity.Price = CommonProductDetailService.RoundingToDecimal(entity.Price, CurrencyTypeConstant.INR);
            entity.ProductDetailWorkerId = userId;
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            if (modelDto.ProductId.HasValue)
                entity.ProductVariant = new()
                {
                    ProductId = modelDto.ProductId.Value
                };


            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                unitOfWork.ProductDetailRepo.Create(entity);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                entity.ProductDetailLink += "-" + entity.Id;
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                await unitOfWork.CommitAsync().ConfigureAwait(false);

                responseResult.Data = mapper.Map<ProductDetailDto>(entity);
            }
            catch (DbUpdateException ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                string msg = ex.GetFriendlySqlExceptionMessage();
                logger.LogError(ex, "Create ProductDetail failed: {Message}", msg);
                responseResult.ErrorMessage.Add(msg);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Create ProductDetail failed.");
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                responseResult.ErrorMessage.Add("Create ProductDetail failed.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("ProductDetail you want to delete was not found.");
            else if (!isAdmin && entity.ProductDetailWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to delete this productdetail.");
            else
            {
                var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
                var downloadableIds = CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies);
                if (downloadableIds.Contains(entity.DeliveryPolicyId))
                {
                    var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);
                    var orderStatus = orderStatuses.SingleOrDefault(x => x.Name == OrderStatusConstant.OrderCompleted);

                    var orderDetail = await unitOfWork.OrderDetailRepo.GetLatestFirstByProductDetailIdAsync(id, orderStatus.Id).ConfigureAwait(false);
                    if (orderDetail != null)
                    {
                        var deliveryPolicy = deliveryPolicies.SingleOrDefault(x => x.Id == entity.DeliveryPolicyId);
                        var days = (DateTime.UtcNow - orderDetail.UpdatedOn).Value.Days;
                        if (days < deliveryPolicy.DeliveryInDays)
                        {
                            responseResult.ErrorMessage.Add($"Unable to delete this productdetail as it is a {deliveryPolicy.Name} category. {deliveryPolicy.DeliveryInDays - days} day(s) left.");
                            return responseResult;
                        }
                    }
                }

                var imageNames = new List<string>();

                var productdetailImageHelperModels = JsonConvert.DeserializeObject<IEnumerable<ProductDetailImageHelperModel>>(entity.ProductDetailImage?.ImageNames ?? "[]");
                imageNames.AddRange(from productdetailImageHelperModel in productdetailImageHelperModels
                                    select productdetailImageHelperModel.Name);

                unitOfWork.ProductDetailRepo.Delete(entity);

                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                var fileResponse = await fileClient.DeleteProductDetailImagesAsync(imageNames).ConfigureAwait(false);
                if (fileResponse.ErrorMessage.Count > 0)
                {
                    if (rowsChanged > 0)
                        responseResult.ErrorMessage.Add("ProductDetail is deleted but unable to delete its all images!");

                    foreach (var error in fileResponse.ErrorMessage)
                    {
                        responseResult.ErrorMessage.Add(error);
                    }
                    return responseResult;
                }

                if (entity.ProductDetailDocument != null)
                {
                    var documentFileResponse = await fileClient.DeleteProductDetailDocumentsAsync([entity.ProductDetailDocument.Document]).ConfigureAwait(false);
                    if (fileResponse.ErrorMessage.Count > 0)
                    {
                        if (rowsChanged > 0)
                            responseResult.ErrorMessage.Add("ProductDetail is deleted but unable to delete its all documents!");

                        foreach (var error in fileResponse.ErrorMessage)
                        {
                            responseResult.ErrorMessage.Add(error);
                        }
                        return responseResult;
                    }
                }

                if (rowsChanged > 0)
                    responseResult.Data = true;
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<GetProductDetailDataTableDto>> GetAllAsync(ProductDetailDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null)
        {
            var model = mapper.Map<ProductDetailDataTableRequest>(modelDto);

            if (isAdmin.HasValue && !isAdmin.Value)
                model.ProductDetailWorkerId = userId;
            var responseResult = new DataTableResponseDto<GetProductDetailDataTableDto>
            {
                AaData = mapper.Map<List<GetProductDetailDataTableDto>>(await unitOfWork.ProductDetailRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.ProductDetailRepo.GetTotalCountAsync().ConfigureAwait(false)
            };

            var shopCategories = await shopCategoryService.GetAllAsync().ConfigureAwait(false);
            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);

            var userIds = responseResult.AaData.SelectMany(x => new List<string> { x.ProductDetailWorkerId, x.PublishedById }).Distinct();
            var applicationUsers = await applicationUserService.GetUsersByUserIdsAsync(userIds).ConfigureAwait(false);

            foreach (var productdetail in responseResult.AaData)
            {
                productdetail.ShopCategory = shopCategories.SingleOrDefault(x => x.Id == productdetail.ShopCategoryId);
                if (CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies).Contains(productdetail.DeliveryPolicyId))
                {
                    productdetail.StockForView = "Not Applicable";
                }
                productdetail.ProductDetailWorker = applicationUsers.SingleOrDefault(x => x.Id == productdetail.ProductDetailWorkerId);
                if (!string.IsNullOrEmpty(productdetail.PublishedById))
                    productdetail.PublishedBy = applicationUsers.SingleOrDefault(x => x.Id == productdetail.PublishedById);
            }

            if (!string.IsNullOrEmpty(modelDto.Search?.Value) || !string.IsNullOrEmpty(modelDto.ProductDetailWorkerId) || modelDto.LastViewedOnStart.HasValue || modelDto.LastViewedOnEnd.HasValue)
                responseResult.RecordsFiltered = await unitOfWork.ProductDetailRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdateProductDetailDto>> GetForEditAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<GetUpdateProductDetailDto>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("ProductDetail not found by supplied Id.");
            else if (!isAdmin && entity.ProductDetailWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
            else
            {
                var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
                responseResult.Data = mapper.Map<GetUpdateProductDetailDto>(entity);
                responseResult.Data.DeliveryPolicy = deliveryPolicies.SingleOrDefault(x => x.Id == entity.DeliveryPolicyId);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> PublishProductDetailAsync(int productdetailId, PublishProductDetailDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(productdetailId).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
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

            if (entity.IsPublished && !entity.IsAvailable)
            {
                responseResult.ErrorMessage.Add("Product marked as unavailable.");
                return responseResult;
            }

            if (entity.IsPublished && string.IsNullOrEmpty(entity.Description))
            {
                responseResult.ErrorMessage.Add("Please provide the description of the productdetail.");
                return responseResult;
            }

            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            var deliveryPolicy = deliveryPolicies.SingleOrDefault(x => x.Id == entity.DeliveryPolicyId);
            if (entity.IsPublished && CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies).Contains(deliveryPolicy.Id) && entity.ProductDetailDocument == null)
            {
                responseResult.ErrorMessage.Add($"Please upload the document as it is a {deliveryPolicy.Name} category.");
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

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int productdetailId, UpdateProductDetailDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();
            if (modelDto.Price > modelDto.Mrp)
            {
                responseResult.ErrorMessage.Add("Price should not be greater than MRP.");
                return responseResult;
            }

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(productdetailId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
                return responseResult;
            }

            modelDto.HtmlEncodeObject();
            mapper.Map(modelDto, entity);
            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            var deliveryPolicy = deliveryPolicies.SingleOrDefault(x => x.Id == entity.DeliveryPolicyId);
            if (CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies).Contains(deliveryPolicy.Id))
                entity.Stock = 0;

            entity.Mrp = CommonProductDetailService.RoundingToDecimal(entity.Mrp, CurrencyTypeConstant.INR);
            entity.Price = CommonProductDetailService.RoundingToDecimal(entity.Price, CurrencyTypeConstant.INR);
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            if (modelDto.ProductId.HasValue)
            {
                if (entity.ProductVariant != null)
                {
                    entity.ProductVariant.ProductId = modelDto.ProductId.Value;
                }
                else
                {
                    entity.ProductVariant = new()
                    {
                        ProductId = modelDto.ProductId.Value
                    };
                }
            }
            else
            {
                if (entity.ProductVariant != null)
                {
                    entity.ProductVariant = null;
                }
            }

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
                logger.LogError(ex, "Error updaing productdetail: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int productdetailId, UpdateProductDetailDescriptionDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(productdetailId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
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

        public async Task<ResponseMessageDto<bool>> UpdateProductDetailWorkerAsync(int productdetailId, UpdateProductDetailWorkerDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(productdetailId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
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

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int productdetailId, UpdateProductDetailMainImageDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(productdetailId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
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

        public async Task<ResponseMessageDto<bool>> UpdateProductDetailImageAsync(int productdetailId, UpdateProductDetailImageDto modelDto, string userId, bool isAdmin)
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

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(productdetailId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
                return responseResult;
            }

            var productdetailImageHelperModels = JsonConvert.DeserializeObject<List<ProductDetailImageHelperModel>>(entity.ProductDetailImage?.ImageNames ?? "[]");

            var fileResponse = await fileClient.UploadProductDetailImageAsync(modelDto.ImageFile).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            productdetailImageHelperModels.Add(new ProductDetailImageHelperModel
            {
                Name = fileResponse.Data
            });
            var imageNames = JsonConvert.SerializeObject(productdetailImageHelperModels);

            modelDto.HtmlEncodeObject();
            if (entity.ProductDetailImage != null)
            {
                entity.ProductDetailImage.ImageNames = imageNames;
                entity.ProductDetailImage.IpAddress = modelDto.IpAddress;
                entity.ProductDetailImage.UpdatedById = userId;
                entity.ProductDetailImage.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ProductDetailImage = new()
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

        public async Task<ResponseMessageDto<bool>> DeleteProductDetailImageAsync(int id, string imageName, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail not found for the deletion of image.");
                return responseResult;
            }
            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this productdetail.");
                return responseResult;
            }

            var productdetailImageHelperModels = JsonConvert.DeserializeObject<List<ProductDetailImageHelperModel>>(entity.ProductDetailImage?.ImageNames ?? "[]");
            var productdetailImageHelperModel = productdetailImageHelperModels.SingleOrDefault(x => x.Name == imageName);
            if (productdetailImageHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Image not found to delete.");
                return responseResult;
            }
            productdetailImageHelperModels.Remove(productdetailImageHelperModel);

            var fileResponse = await fileClient.DeleteProductDetailImagesAsync([imageName]).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            if (productdetailImageHelperModels.Count == 0)
                entity.ProductDetailImage = null;
            else
            {
                var imageNames = JsonConvert.SerializeObject(productdetailImageHelperModels);

                entity.ProductDetailImage.ImageNames = imageNames;
                entity.UpdatedById = userId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductDetailDocumentAsync(int productdetailId, UpdateProductDetailDocumentDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            if (!await fileValidationService.ValidateDocumentFileSizeAsync(modelDto.DocumentFile).ConfigureAwait(false))
            {
                responseResult.ErrorMessage.Add(fileValidationService.DocumentFileSizeInvalidMsg);
                return responseResult;
            }
            if (!await fileValidationService.ValidateDocumentFileExtensionAsync(modelDto.DocumentFile).ConfigureAwait(false))
            {
                responseResult.ErrorMessage.Add(fileValidationService.DocumentFileExtensionInValidMsg);
                responseResult.ErrorMessage.Add(fileValidationService.ImageFileExtensionInValidMsg);
                return responseResult;
            }

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(productdetailId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this productdetail.");
                return responseResult;
            }

            if (entity.ProductDetailDocument != null)
            {
                responseResult.ErrorMessage.Add("Document is already uploaded. Delete the existing first.");
                return responseResult;
            }

            var fileResponse = await fileClient.UploadProductDetailDocumentAsync(modelDto.DocumentFile).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            modelDto.HtmlEncodeObject();
            entity.ProductDetailDocument = new()
            {
                Id = entity.Id,
                Document = fileResponse.Data,
                IpAddress = modelDto.IpAddress,
                CreatedById = userId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            entity.IpAddress = modelDto.IpAddress;
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductDetailDocumentAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail not found for the deletion of document.");
                return responseResult;
            }
            if (!isAdmin && entity.ProductDetailWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this productdetail.");
                return responseResult;
            }

            if (entity.ProductDetailDocument == null)
            {
                responseResult.ErrorMessage.Add("Document not found to delete.");
                return responseResult;
            }

            var fileResponse = await fileClient.DeleteProductDetailDocumentsAsync([entity.ProductDetailDocument.Document]).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            entity.ProductDetailDocument = null;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<IEnumerable<GetProductDetailForListViewDto>> GetForListViewAsync(ProductDetailDataTableForViewRequestDto modelDto)
        {
            return await CommonProductDetailService.GetForListViewAsync(unitOfWork, mapper, shopCategoryService, config, modelDto, dropDownService).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<GetForViewProductDetailDto>> GetForViewAsync(string subCategoryName, string productdetailLink)
        {
            var responseResult = new ResponseMessageDto<GetForViewProductDetailDto>();

            var shopCategories = await shopCategoryService.GetAllAsync().ConfigureAwait(false);

            var shopCategoryId = shopCategories.Where(x => x.Name.Equals(subCategoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Id;

            if (!shopCategoryId.HasValue)
            {
                responseResult.ErrorMessage.Add("ProductDetail not found by supplied shopCategoryName.");
                return responseResult;
            }

            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            var downloadableIds = CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies);

            var entity = await unitOfWork.ProductDetailRepo.GetForViewAsync(shopCategoryId.Value, productdetailLink, downloadableIds).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("ProductDetail not found by supplied productdetailLink.");
            else
            {
                var exchangePolicies = await dropDownService.GetExchangePoliciesAsync().ConfigureAwait(false);
                var returnPolicies = await dropDownService.GetReturnPoliciesAsync().ConfigureAwait(false);

                responseResult.Data = mapper.Map<GetForViewProductDetailDto>(entity);
                responseResult.Data.Mrp = CommonProductDetailService.RoundingToDecimal(responseResult.Data.Mrp, responseResult.Data.CurrencyLetter);
                responseResult.Data.Price = CommonProductDetailService.RoundingToDecimal(responseResult.Data.Price, responseResult.Data.CurrencyLetter);

                if (entity.ExchangePolicyId > 0)
                {
                    var exchangePolicy = exchangePolicies.SingleOrDefault(x => x.Id == entity.ExchangePolicyId);
                    if (exchangePolicy == null)
                    {
                        responseResult.ErrorMessage.Add("Problem in fetching product detail.");
                        return responseResult;
                    }
                    responseResult.Data.ExchangePolicy = exchangePolicy;
                }
                if (entity.DeliveryPolicyId > 0)
                {
                    var deliveryPolicy = deliveryPolicies.SingleOrDefault(x => x.Id == entity.DeliveryPolicyId);
                    if (deliveryPolicy == null)
                    {
                        responseResult.ErrorMessage.Add("Problem in fetching product detail.");
                        return responseResult;
                    }
                    responseResult.Data.DeliveryPolicy = deliveryPolicy;
                }
                if (entity.ReturnPolicyId > 0)
                {
                    var returnPolicy = returnPolicies.SingleOrDefault(x => x.Id == entity.ExchangePolicyId);
                    if (returnPolicy == null)
                    {
                        responseResult.ErrorMessage.Add("Problem in fetching product detail.");
                        return responseResult;
                    }
                    responseResult.Data.ReturnPolicy = returnPolicy;
                }

                if (entity.ProductVariant != null && entity.ProductVariant.Product != null && entity.ProductVariant.Product.ProductVariants.Count > 0)
                {
                    foreach (var variant in entity.ProductVariant.Product.ProductVariants)
                    {
                        responseResult.Data.ProductVariants.Add(CommonProductDetailService.SetProductDetailForListViewDto(config, shopCategories, variant.ProductDetail));
                    }
                }

                var description = new StringBuilder(responseResult.Data.Description);
                if (webHostEnvironment.IsProduction())
                {
                    foreach (var replacement in DescriptionHelper.ReplacementsProdEnv(config))
                    {
                        description.Replace(replacement.Key, replacement.Value);
                    }
                }
                else
                {
                    foreach (var replacement in DescriptionHelper.ReplacementsDevEnv(config))
                    {
                        description.Replace(replacement.Key, replacement.Value);
                    }
                }
                responseResult.Data.Description = description.ToString();
            }

            return responseResult;
        }

        public async Task<string> GetSitemapAsync()
        {
            var model = new ProductDetailDataTableForViewRequest
            {
                Start = 0,
                Length = 15000
            };
            var shopCategories = await shopCategoryService.GetAllAsync().ConfigureAwait(false);
            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            var downloadableIds = CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies);
            var entities = await unitOfWork.ProductDetailRepo.GetForListViewAsync(model, downloadableIds).ConfigureAwait(false);

            var sitemapData = new SitemapDto
            {
                Urls = new()
            };
            var products = CategoryConstant.Products.ToLower();
            foreach (var entity in entities)
            {
                sitemapData.Urls.Add(new UrlData
                {
                    LastMod = entity.UpdatedOn?.ToDateTimeUtcString(),
                    Loc = $"{config.GetValue<string>("SrcApiShopIn:SiteUrl")}/{products}/{shopCategories.FirstOrDefault(x => x.Id == entity.ShopCategoryId)?.Name}/{entity.ProductDetailLink}".ToLower()
                });
            }
            sitemapData.Urls.Add(new UrlData
            {
                LastMod = DateTime.UtcNow.ToDateTimeUtcString(),
                Loc = $"{config.GetValue<string>("SrcApiShopIn:SiteUrl")}/{products}"
            });
            return sitemapData.SerializeToXml();
        }

        public async Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null)
        {
            return await unitOfWork.ProductDetailRepo.GetTotalUniquePagesVisitedAsync(lastViewedOnStart, lastViewedOnEnd).ConfigureAwait(false);
        }

        public async Task<int> GetTotalPagesVisitedLifetimeAsync()
        {
            return await unitOfWork.ProductDetailRepo.GetTotalPagesVisitedLifetimeAsync().ConfigureAwait(false);
        }

        public async Task IncrementProductDetailView2ForProductDetailIdsAsync(Dictionary<int, int> data)
        {
            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var entities = await unitOfWork.ProductDetailRepo.GetProductDetailViewWithLockByProductDetailIdsAsync(data.Select(x => x.Key)).ConfigureAwait(false);

                foreach (var item in data)
                {
                    var datetime = DateTime.UtcNow;
                    var entity = entities.FirstOrDefault(x => x.Id == item.Key);
                    if (entity != null)
                    {
                        entity.TotalViews += item.Value;
                        entity.LastViewedOn = datetime;
                    }
                    else
                    {
                        unitOfWork.ProductDetailRepo.AddProductDetailView(new()
                        {
                            Id = item.Key,
                            TotalViews = item.Value,
                            LastViewedOn = datetime
                        });
                    }
                }
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                await unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Exception occurred while incrementing productdetail views.");
            }
        }

        public async Task<ResponseMessageDto<DownloadDocumentDto>> GetProductDocumentDetailAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<DownloadDocumentDto>();

            var entity = await unitOfWork.ProductDetailRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("ProductDetail was not found.");
            else if (entity.ProductDetailDocument == null)
                responseResult.ErrorMessage.Add("Document was not found for the productdetail.");
            else if (!isAdmin && entity.ProductDetailWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to download productdetail document.");
            else
            {
                var dateUtc = DateTime.UtcNow;
                var downloadDocumentDto = new DownloadDocumentDto
                {
                    Token = fileClient.GetEncryptedFileToken(dateUtc, RoleConstant.Administrator),
                    FileUrl = fileClient.DownloadDocumentUrl,
                    EncryptedFileName = fileClient.GetEncryptedFileName(entity.ProductDetailDocument.Document, dateUtc),
                    AuthType = "Basic"
                };
                responseResult.Data = downloadDocumentDto;
            }

            return responseResult;
        }
    }
}
