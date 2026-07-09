using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using Ts.Application.AppConstants;
using Ts.Application.HelperExtensions;
using Ts.Common.AppInterfaces;
using Ts.Common.Constant.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.DotIn.Domain.DataTableModels.PostDataTables;
using Ts.DotIn.Domain.HelperModels;
using Ts.DotIn.Domain.Interfaces;
using Ts.DotIn.Domain.Models;
using Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotIn.Dto.PostDtos;
using Ts.DotIn.Dto.PostImageDtos;
using Ts.DotIn.Dto.PostRelativeDtos;
using Ts.DotIn.Service.DataInterfaces;
using Ts.DotIn.Service.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Service.PostHelpers;
using Ts.Dto;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.DotIn.Service.DataServices
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<PostService> logger;
        private readonly IFileClient fileClient;
        private readonly IFileValidationService fileValidationService;
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger,
            IFileClient fileClient, IFileValidationService fileValidationService,
            ICategoryService categoryService, ISubCategoryService subCategoryService,
            IApplicationUserService applicationUserService, IConfiguration config,
            IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.fileClient = fileClient;
            this.fileValidationService = fileValidationService;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
            this.applicationUserService = applicationUserService;
            this.config = config;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResponseMessageDto<PostDto>> CreateAsync(CreatePostDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<PostDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<Post>(modelDto);
            entity.PostWorkerId = userId;
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                unitOfWork.PostRepo.Create(entity);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                entity.PostLink += "-" + entity.Id;
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                await unitOfWork.CommitAsync().ConfigureAwait(false);

                responseResult.Data = mapper.Map<PostDto>(entity);
            }
            catch (DbUpdateException ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                string msg = ex.GetFriendlySqlExceptionMessage();
                logger.LogError(ex, "Create Post faild: {Message}", msg);
                responseResult.ErrorMessage.Add(msg);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Create Post failed.");
                responseResult.ErrorMessage.Add("Create Post failed.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Post you want to delete was not found.");
            else if (!isAdmin && entity.PostWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to delete this post.");
            else
            {
                var imageNames = new List<string>();

                var postImageHelperModels = JsonConvert.DeserializeObject<IEnumerable<PostImageHelperModel>>(entity.PostImage?.ImageNames ?? "[]");
                imageNames.AddRange(from postImageHelperModel in postImageHelperModels
                                    select postImageHelperModel.Name);

                unitOfWork.PostRepo.Delete(entity);

                try
                {
                    var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                    var fileResponse = await fileClient.DeletePostImagesAsync(imageNames).ConfigureAwait(false);
                    if (fileResponse.ErrorMessage.Count > 0)
                    {
                        if (rowsChanged > 0)
                            responseResult.ErrorMessage.Add("Post is deleted but unable to delete its all images!");

                        foreach (var error in fileResponse.ErrorMessage)
                        {
                            responseResult.ErrorMessage.Add(error);
                        }
                        return responseResult;
                    }

                    if (rowsChanged > 0)
                        responseResult.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    string msg = ex.GetFriendlySqlExceptionMessage();
                    responseResult.ErrorMessage.Add(msg);
                    responseResult.ErrorMessage.Add("If you are deleting, please check Relative Url, delete from every websites first!");
                    logger.LogError(ex, "Error deleting post: {Message}", msg);
                }
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<GetPostDataTableDto>> GetAllAsync(PostDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null)
        {
            var model = mapper.Map<PostDataTableRequest>(modelDto);

            if (isAdmin.HasValue && !isAdmin.Value)
                model.PostWorkerId = userId;
            var responseResult = new DataTableResponseDto<GetPostDataTableDto>
            {
                AaData = mapper.Map<List<GetPostDataTableDto>>(await unitOfWork.PostRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.PostRepo.GetTotalCountAsync().ConfigureAwait(false)
            };

            var categories = await categoryService.GetAllAsync().ConfigureAwait(false);
            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);

            var userIds = responseResult.AaData.SelectMany(x => new List<string> { x.PostWorkerId, x.PublishedById }).Distinct();
            var applicationUsers = await applicationUserService.GetUsersByUserIdsAsync(userIds).ConfigureAwait(false);

            foreach (var post in responseResult.AaData)
            {
                post.Category = categories.SingleOrDefault(x => x.Id == post.CategoryId);
                post.SubCategory = subCategories.SingleOrDefault(x => x.Id == post.SubCategoryId);
                post.PostWorker = applicationUsers.SingleOrDefault(x => x.Id == post.PostWorkerId);
                if (!string.IsNullOrEmpty(post.PublishedById))
                    post.PublishedBy = applicationUsers.SingleOrDefault(x => x.Id == post.PublishedById);
            }

            if (!string.IsNullOrEmpty(modelDto.Search?.Value) || !string.IsNullOrEmpty(modelDto.PostWorkerId) || modelDto.LastViewedOnStart.HasValue || modelDto.LastViewedOnEnd.HasValue)
                responseResult.RecordsFiltered = await unitOfWork.PostRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdatePostDto>> GetForEditAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<GetUpdatePostDto>();

            var entity = await unitOfWork.PostRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Post not found by supplied Id.");
            else if (!isAdmin && entity.PostWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
            else
                responseResult.Data = mapper.Map<GetUpdatePostDto>(entity);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> PublishPostAsync(int postId, PublishPostDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(postId).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
                return responseResult;
            }

            if ((modelDto.IsRepublish && entity.PublishedById != null) || (!entity.IsPublished && entity.PublishedById == null))
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
                responseResult.ErrorMessage.Add("Please provide the description of the post.");
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

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int postId, UpdatePostDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(postId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
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
                logger.LogError(ex, "Error updating post: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int postId, UpdatePostDescriptionDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(postId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
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

        public async Task<ResponseMessageDto<bool>> UpdatePostWorkerAsync(int postId, UpdatePostWorkerDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(postId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
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

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int postId, UpdatePostMainImageDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(postId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
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

        public async Task<ResponseMessageDto<bool>> UpdatePostImageAsync(int postId, UpdatePostImageDto modelDto, string userId, bool isAdmin)
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

            var entity = await unitOfWork.PostRepo.GetAsync(postId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
                return responseResult;
            }

            var postImageHelperModels = JsonConvert.DeserializeObject<List<PostImageHelperModel>>(entity.PostImage?.ImageNames ?? "[]");

            var fileResponse = await fileClient.UploadPostImageAsync(modelDto.ImageFile).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            postImageHelperModels.Add(new PostImageHelperModel
            {
                Name = fileResponse.Data,
                Source = modelDto.ImageSource
            });
            var imageNames = JsonConvert.SerializeObject(postImageHelperModels);

            modelDto.HtmlEncodeObject();
            if (entity.PostImage != null)
            {
                entity.PostImage.ImageNames = imageNames;
                entity.PostImage.IpAddress = modelDto.IpAddress;
                entity.PostImage.UpdatedById = userId;
                entity.PostImage.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.PostImage = new()
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

        public async Task<ResponseMessageDto<bool>> DeletePostImageAsync(int id, string imageName, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post not found for the deletion of image.");
                return responseResult;
            }
            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this post.");
                return responseResult;
            }

            var postImageHelperModels = JsonConvert.DeserializeObject<List<PostImageHelperModel>>(entity.PostImage?.ImageNames ?? "[]");
            var postImageHelperModel = postImageHelperModels.SingleOrDefault(x => x.Name == imageName);
            if (postImageHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Image not found to delete.");
                return responseResult;
            }
            postImageHelperModels.Remove(postImageHelperModel);

            var fileResponse = await fileClient.DeletePostImagesAsync([imageName]).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            if (postImageHelperModels.Count == 0)
                entity.PostImage = null;
            else
            {
                var imageNames = JsonConvert.SerializeObject(postImageHelperModels);

                entity.PostImage.ImageNames = imageNames;
                entity.UpdatedById = userId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdatePostRelativeAsync(int postId, UpdatePostRelativeDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(postId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this post.");
                return responseResult;
            }

            var postRelativeHelperModels = JsonConvert.DeserializeObject<List<PostRelativeHelperModel>>(entity.PostRelative?.RelativeUrl ?? "[]");

            var hrefLang = RelativeHrefLangConstant.GetHrefLangs()[modelDto.HrefLangId];
            if (string.IsNullOrEmpty(hrefLang))
            {
                responseResult.ErrorMessage.Add("HrefLang is not valid.");
                return responseResult;
            }
            else if (postRelativeHelperModels.SingleOrDefault(x => x.HrefLang == hrefLang) != null)
            {
                responseResult.ErrorMessage.Add("HrefLang provided is already present.");
                return responseResult;
            }

            postRelativeHelperModels.Add(new PostRelativeHelperModel
            {
                HrefLang = hrefLang,
                Href = modelDto.Href
            });
            var relativeUrls = JsonConvert.SerializeObject(postRelativeHelperModels);

            modelDto.HtmlEncodeObject();
            if (entity.PostRelative != null)
            {
                entity.PostRelative.RelativeUrl = relativeUrls;
                entity.PostRelative.IpAddress = modelDto.IpAddress;
                entity.PostRelative.UpdatedById = userId;
                entity.PostRelative.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.PostRelative = new PostRelative
                {
                    Id = entity.Id,
                    RelativeUrl = relativeUrls,
                    IpAddress = modelDto.IpAddress,
                    CreatedById = userId,
                    CreatedOn = DateTime.UtcNow
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

        public async Task<ResponseMessageDto<bool>> DeletePostRelativeAsync(int id, string hrefLang, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.PostRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Post not found for the deletion of relative.");
                return responseResult;
            }
            if (!isAdmin && entity.PostWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this post.");
                return responseResult;
            }

            var postRelativeHelperModels = JsonConvert.DeserializeObject<List<PostRelativeHelperModel>>(entity.PostRelative?.RelativeUrl ?? "[]");
            var postRelativeHelperModel = postRelativeHelperModels.SingleOrDefault(x => x.HrefLang == hrefLang);
            if (postRelativeHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Relative Url not found to delete.");
                return responseResult;
            }
            postRelativeHelperModels.Remove(postRelativeHelperModel);

            if (postRelativeHelperModels.Count == 0)
                entity.PostRelative = null;
            else
            {
                var relativeUrls = JsonConvert.SerializeObject(postRelativeHelperModels);

                entity.PostRelative.RelativeUrl = relativeUrls;
                entity.UpdatedById = userId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<IEnumerable<GetPostForListViewDto>> GetForListViewAsync(PostDataTableForViewRequestDto modelDto)
        {
            var dtos = new List<GetPostForListViewDto>();

            var model = mapper.Map<PostDataTableForViewRequest>(modelDto);

            var categories = await categoryService.GetAllAsync().ConfigureAwait(false);
            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(modelDto.CategoryName))
            {
                model.CategoryId = categories.FirstOrDefault(x => x.Name.Equals(modelDto.CategoryName, StringComparison.OrdinalIgnoreCase))?.Id;
                if (model.CategoryId == null)
                    return dtos;
            }
            if (!string.IsNullOrEmpty(modelDto.SubCategoryName))
            {
                model.SubCategoryId = subCategories.FirstOrDefault(x => x.Name.Equals(modelDto.SubCategoryName, StringComparison.OrdinalIgnoreCase))?.Id;
                if (model.SubCategoryId == null)
                    return dtos;
            }

            var entities = await unitOfWork.PostRepo.GetForListViewAsync(model).ConfigureAwait(false);

            foreach (var entity in entities)
            {
                var dto = new GetPostForListViewDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    CategoryId = entity.CategoryId,
                    CategoryName = categories.FirstOrDefault(x => x.Id == entity.CategoryId).Name,
                    SubCategoryId = entity.SubCategoryId,
                    SubCategoryName = subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId).Name,
                    PostLink = entity.PostLink,
                    MainImageUrl = entity.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:PostImageUrl")}/{entity.MainImage}",
                    PublishedOn = entity.PublishedOn
                };
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<ResponseMessageDto<GetPostForViewDto>> GetForViewAsync(string categoryName, string subCategoryName, string postLink)
        {
            var responseResult = new ResponseMessageDto<GetPostForViewDto>();

            var categories = await categoryService.GetAllAsync().ConfigureAwait(false);
            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);

            var categoryId = categories.Where(x => x.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Id;
            var subCategoryId = subCategories.Where(x => x.Name.Equals(subCategoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Id;

            if (!categoryId.HasValue || !subCategoryId.HasValue)
            {
                responseResult.ErrorMessage.Add("Post not found by supplied categoryName or subCategoryName.");
                return responseResult;
            }

            var entity = await unitOfWork.PostRepo.GetForViewAsync(categoryId.Value, subCategoryId.Value, postLink).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Post not found by supplied postLink.");
            else
            {
                responseResult.Data = mapper.Map<GetPostForViewDto>(entity);
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
            var model = new PostDataTableForViewRequest
            {
                Start = 0,
                Length = 15000
            };
            var categories = await categoryService.GetAllAsync().ConfigureAwait(false);
            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);
            var entities = await unitOfWork.PostRepo.GetForListViewAsync(model).ConfigureAwait(false);

            var sitemapData = new SitemapDto
            {
                Urls = new()
            };
            foreach (var entity in entities)
            {
                sitemapData.Urls.Add(new UrlData
                {
                    LastMod = entity.UpdatedOn?.ToDateTimeUtcString(),
                    Loc = $"{config.GetValue<string>("SrcApiDotIn:SiteUrl")}/{categories.FirstOrDefault(x => x.Id == entity.CategoryId)?.Name}/{subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId)?.Name}/{entity.PostLink}".ToLower()
                });
            }
            sitemapData.Urls.Add(new UrlData
            {
                LastMod = DateTime.UtcNow.ToDateTimeUtcString(),
                Loc = config.GetValue<string>("SrcApiDotIn:SiteUrl")
            });
            return sitemapData.SerializeToXml();
        }

        public async Task IncrementPostViewForPostIdsAsync(IEnumerable<int> postIds)
        {
            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var groupedIds = postIds.GroupBy(id => id)
                                        .Select(group => new { Id = group.Key, Count = group.Count() });
                var entities = await unitOfWork.PostRepo.GetPostViewWithLockByPostIdsAsync(groupedIds.Select(x => x.Id)).ConfigureAwait(false);

                foreach (var group in groupedIds)
                {
                    var datetime = DateTime.UtcNow;
                    var entity = entities.FirstOrDefault(x => x.Id == group.Id);
                    if (entity != null)
                    {
                        entity.TotalViews += group.Count;
                        entity.LastViewedOn = datetime;
                    }
                    else
                    {
                        unitOfWork.PostRepo.AddPostView(new()
                        {
                            Id = group.Id,
                            TotalViews = group.Count,
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
                logger.LogError(ex, "Exception occurred while incrementing post views.");
            }
        }

        public async Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null)
        {
            return await unitOfWork.PostRepo.GetTotalUniquePagesVisitedAsync(lastViewedOnStart, lastViewedOnEnd).ConfigureAwait(false);
        }

        public async Task<int> GetTotalPagesVisitedLifetimeAsync()
        {
            return await unitOfWork.PostRepo.GetTotalPagesVisitedLifetimeAsync().ConfigureAwait(false);
        }

        public async Task IncrementPostView2ForPostIdsAsync(Dictionary<int, int> data)
        {
            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var entities = await unitOfWork.PostRepo.GetPostViewWithLockByPostIdsAsync(data.Select(x => x.Key)).ConfigureAwait(false);

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
                        unitOfWork.PostRepo.AddPostView(new()
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
                logger.LogError(ex, "Exception occurred while incrementing post views.");
            }
        }
    }
}
