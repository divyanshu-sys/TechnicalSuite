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
using Ts.ShopIn.Domain.DataTableModels.BlogDataTables;
using Ts.ShopIn.Domain.HelperModels;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.BlogImageDtos;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
using Ts.ShopIn.Service.DataInterfaces;
using Ts.ShopIn.Service.Helpers;
using Ts.ShopIn.Service.HttpClientServices.ClientInterfaces;
namespace Ts.ShopIn.Service.DataServices
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<BlogService> logger;
        private readonly IFileClient fileClient;
        private readonly IFileValidationService fileValidationService;
        private readonly ISubCategoryService subCategoryService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BlogService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BlogService> logger,
            IFileClient fileClient, IFileValidationService fileValidationService,
            ISubCategoryService subCategoryService,
            IApplicationUserService applicationUserService, IConfiguration config,
            IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.fileClient = fileClient;
            this.fileValidationService = fileValidationService;
            this.subCategoryService = subCategoryService;
            this.applicationUserService = applicationUserService;
            this.config = config;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResponseMessageDto<BlogDto>> CreateAsync(CreateBlogDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<BlogDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<Blog>(modelDto);
            entity.BlogWorkerId = userId;
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                unitOfWork.BlogRepo.Create(entity);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                entity.BlogLink += "-" + entity.Id;
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                await unitOfWork.CommitAsync().ConfigureAwait(false);

                responseResult.Data = mapper.Map<BlogDto>(entity);
            }
            catch (DbUpdateException ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                string msg = ex.GetFriendlySqlExceptionMessage();
                logger.LogError(ex, "Create Blog failed: {Message}", msg);
                responseResult.ErrorMessage.Add(msg);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Create Blog failed.");
                responseResult.ErrorMessage.Add("Create Blog failed.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.BlogRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Blog you want to delete was not found.");
            else if (!isAdmin && entity.BlogWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to delete this blog.");
            else
            {
                var imageNames = new List<string>();

                var blogImageHelperModels = JsonConvert.DeserializeObject<IEnumerable<BlogImageHelperModel>>(entity.BlogImage?.ImageNames ?? "[]");
                imageNames.AddRange(from blogImageHelperModel in blogImageHelperModels
                                    select blogImageHelperModel.Name);

                unitOfWork.BlogRepo.Delete(entity);

                var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                var fileResponse = await fileClient.DeleteBlogImagesAsync(imageNames).ConfigureAwait(false);
                if (fileResponse.ErrorMessage.Count > 0)
                {
                    if (rowsChanged > 0)
                        responseResult.ErrorMessage.Add("Blog is deleted but unable to delete its all images!");

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

        public async Task<DataTableResponseDto<GetBlogDataTableDto>> GetAllAsync(BlogDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null)
        {
            var model = mapper.Map<BlogDataTableRequest>(modelDto);

            if (isAdmin.HasValue && !isAdmin.Value)
                model.BlogWorkerId = userId;
            var responseResult = new DataTableResponseDto<GetBlogDataTableDto>
            {
                AaData = mapper.Map<List<GetBlogDataTableDto>>(await unitOfWork.BlogRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.BlogRepo.GetTotalCountAsync().ConfigureAwait(false)
            };

            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);

            var userIds = responseResult.AaData.SelectMany(x => new List<string> { x.BlogWorkerId, x.PublishedById }).Distinct();
            var applicationUsers = await applicationUserService.GetUsersByUserIdsAsync(userIds).ConfigureAwait(false);

            foreach (var blog in responseResult.AaData)
            {
                blog.SubCategory = subCategories.SingleOrDefault(x => x.Id == blog.SubCategoryId);
                blog.BlogWorker = applicationUsers.SingleOrDefault(x => x.Id == blog.BlogWorkerId);
                if (!string.IsNullOrEmpty(blog.PublishedById))
                    blog.PublishedBy = applicationUsers.SingleOrDefault(x => x.Id == blog.PublishedById);
            }

            if (!string.IsNullOrEmpty(modelDto.Search?.Value) || !string.IsNullOrEmpty(modelDto.BlogWorkerId) || modelDto.LastViewedOnStart.HasValue || modelDto.LastViewedOnEnd.HasValue)
                responseResult.RecordsFiltered = await unitOfWork.BlogRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdateBlogDto>> GetForEditAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<GetUpdateBlogDto>();

            var entity = await unitOfWork.BlogRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Blog not found by supplied Id.");
            else if (!isAdmin && entity.BlogWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to update this blog.");
            else
                responseResult.Data = mapper.Map<GetUpdateBlogDto>(entity);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> PublishBlogAsync(int blogId, PublishBlogDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.BlogRepo.GetAsync(blogId).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Blog does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.BlogWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this blog.");
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

            if (entity.IsPublished && string.IsNullOrEmpty(entity.Description))
            {
                responseResult.ErrorMessage.Add("Please provide the description of the blog.");
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

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int blogId, UpdateBlogDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.BlogRepo.GetAsync(blogId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Blog does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.BlogWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this blog.");
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
                logger.LogError(ex, "Error updating blog: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int blogId, UpdateBlogDescriptionDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.BlogRepo.GetAsync(blogId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Blog does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.BlogWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this blog.");
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

        public async Task<ResponseMessageDto<bool>> UpdateBlogWorkerAsync(int blogId, UpdateBlogWorkerDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.BlogRepo.GetAsync(blogId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Blog does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.BlogWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this blog.");
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

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int blogId, UpdateBlogMainImageDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.BlogRepo.GetAsync(blogId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Blog does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.BlogWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this blog.");
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

        public async Task<ResponseMessageDto<bool>> UpdateBlogImageAsync(int blogId, UpdateBlogImageDto modelDto, string userId, bool isAdmin)
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

            var entity = await unitOfWork.BlogRepo.GetAsync(blogId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Blog does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.BlogWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this blog.");
                return responseResult;
            }

            var blogImageHelperModels = JsonConvert.DeserializeObject<List<BlogImageHelperModel>>(entity.BlogImage?.ImageNames ?? "[]");

            var fileResponse = await fileClient.UploadBlogImageAsync(modelDto.ImageFile).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            blogImageHelperModels.Add(new BlogImageHelperModel
            {
                Name = fileResponse.Data,
                Source = modelDto.ImageSource
            });
            var imageNames = JsonConvert.SerializeObject(blogImageHelperModels);

            modelDto.HtmlEncodeObject();
            if (entity.BlogImage != null)
            {
                entity.BlogImage.ImageNames = imageNames;
                entity.BlogImage.IpAddress = modelDto.IpAddress;
                entity.BlogImage.UpdatedById = userId;
                entity.BlogImage.UpdatedOn = DateTime.UtcNow;

            }
            else
            {
                entity.BlogImage = new()
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

        public async Task<ResponseMessageDto<bool>> DeleteBlogImageAsync(int id, string imageName, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.BlogRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Blog not found for the deletion of image.");
                return responseResult;
            }
            if (!isAdmin && entity.BlogWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this blog.");
                return responseResult;
            }

            var blogImageHelperModels = JsonConvert.DeserializeObject<List<BlogImageHelperModel>>(entity.BlogImage?.ImageNames ?? "[]");
            var blogImageHelperModel = blogImageHelperModels.SingleOrDefault(x => x.Name == imageName);
            if (blogImageHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Image not found to delete.");
                return responseResult;
            }
            blogImageHelperModels.Remove(blogImageHelperModel);

            var fileResponse = await fileClient.DeleteBlogImagesAsync([imageName]).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            if (blogImageHelperModels.Count == 0)
                entity.BlogImage = null;
            else
            {
                var imageNames = JsonConvert.SerializeObject(blogImageHelperModels);

                entity.BlogImage.ImageNames = imageNames;
                entity.UpdatedById = userId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<IEnumerable<GetBlogForListViewDto>> GetForListViewAsync(BlogDataTableForViewRequestDto modelDto)
        {
            return await CommonBlogService.GetForListViewAsync(unitOfWork, mapper, subCategoryService, config, modelDto).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<BlogDto>> GetForViewAsync(string subCategoryName, string blogLink)
        {
            var responseResult = new ResponseMessageDto<BlogDto>();

            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);

            var subCategoryId = subCategories.Where(x => x.Name.Equals(subCategoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Id;

            if (!subCategoryId.HasValue)
            {
                responseResult.ErrorMessage.Add("Blog not found by supplied subCategoryName.");
                return responseResult;
            }

            var entity = await unitOfWork.BlogRepo.GetForViewAsync(subCategoryId.Value, blogLink).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Blog not found by supplied blogLink.");
            else
            {
                responseResult.Data = mapper.Map<BlogDto>(entity);
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
            var model = new BlogDataTableForViewRequest
            {
                Start = 0,
                Length = 15000
            };
            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);
            var entities = await unitOfWork.BlogRepo.GetForListViewAsync(model).ConfigureAwait(false);

            var sitemapData = new SitemapDto
            {
                Urls = new()
            };
            var blogs = CategoryConstant.Blogs.ToLower();
            foreach (var entity in entities)
            {
                sitemapData.Urls.Add(new UrlData
                {
                    LastMod = entity.UpdatedOn?.ToDateTimeUtcString(),
                    Loc = $"{config.GetValue<string>("SrcApiShopIn:SiteUrl")}/{blogs}/{subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId)?.Name}/{entity.BlogLink}".ToLower()
                });
            }
            sitemapData.Urls.Add(new UrlData
            {
                LastMod = DateTime.UtcNow.ToDateTimeUtcString(),
                Loc = config.GetValue<string>("SrcApiShopIn:SiteUrl")
            });
            return sitemapData.SerializeToXml();
        }

        public async Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null)
        {
            return await unitOfWork.BlogRepo.GetTotalUniquePagesVisitedAsync(lastViewedOnStart, lastViewedOnEnd).ConfigureAwait(false);
        }

        public async Task<int> GetTotalPagesVisitedLifetimeAsync()
        {
            return await unitOfWork.BlogRepo.GetTotalPagesVisitedLifetimeAsync().ConfigureAwait(false);
        }

        public async Task IncrementBlogView2ForBlogIdsAsync(Dictionary<int, int> data)
        {
            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var entities = await unitOfWork.BlogRepo.GetBlogViewWithLockByBlogIdsAsync(data.Select(x => x.Key)).ConfigureAwait(false);

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
                        unitOfWork.BlogRepo.AddBlogView(new()
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
                logger.LogError(ex, "Exception occurred while incrementing blog views.");
            }
        }
    }
}
