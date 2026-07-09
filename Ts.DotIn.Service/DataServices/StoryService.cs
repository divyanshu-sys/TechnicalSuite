using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using Ts.Application.AppConstants;
using Ts.Application.HelperExtensions;
using Ts.Common.AppInterfaces;
using Ts.Common.Constant.AppConstants;
using Ts.Common.Constant.SiteConstants;
using Ts.Common.HelperExtensions;
using Ts.DotIn.Domain.DataTableModels.StoryDataTables;
using Ts.DotIn.Domain.HelperModels;
using Ts.DotIn.Domain.Interfaces;
using Ts.DotIn.Domain.Models;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotIn.Dto.StoryDtos;
using Ts.DotIn.Dto.StoryImageDtos;
using Ts.DotIn.Dto.StoryRelativeDtos;
using Ts.DotIn.Service.DataInterfaces;
using Ts.DotIn.Service.HttpClientServices.ClientInterfaces;
using Ts.Dto;
using Ts.Service.DataInterfaces;
using Ts.Service.HelperExtensions;
namespace Ts.DotIn.Service.DataServices
{
    public class StoryService : IStoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<StoryService> logger;
        private readonly IFileClient fileClient;
        private readonly IFileValidationService fileValidationService;
        private readonly ISubCategoryService subCategoryService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IConfiguration config;

        public StoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StoryService> logger,
            IFileClient fileClient, IFileValidationService fileValidationService,
            ISubCategoryService subCategoryService,
            IApplicationUserService applicationUserService, IConfiguration config)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.fileClient = fileClient;
            this.fileValidationService = fileValidationService;
            this.subCategoryService = subCategoryService;
            this.applicationUserService = applicationUserService;
            this.config = config;
        }

        public async Task<ResponseMessageDto<StoryDto>> CreateAsync(CreateStoryDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<StoryDto>();

            modelDto.HtmlEncodeObject();
            var entity = mapper.Map<Story>(modelDto);
            entity.StoryWorkerId = userId;
            entity.CreatedById = userId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                unitOfWork.StoryRepo.Create(entity);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                entity.StoryLink += "-" + entity.Id;
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                await unitOfWork.CommitAsync().ConfigureAwait(false);

                responseResult.Data = mapper.Map<StoryDto>(entity);
            }
            catch (DbUpdateException ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                string msg = ex.GetFriendlySqlExceptionMessage();
                logger.LogError(ex, "Create Story failed: {Message}", msg);
                responseResult.ErrorMessage.Add(msg);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Create Story failed.");
                responseResult.ErrorMessage.Add("Create Story failed.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Story you want to delete was not found.");
            else if (!isAdmin && entity.StoryWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to delete this story.");
            else
            {
                var imageNames = new List<string>();

                var storyImageHelperModels = JsonConvert.DeserializeObject<IEnumerable<StoryImageHelperModel>>(entity.StoryImage?.ImageNames ?? "[]");
                imageNames.AddRange(from storyImageHelperModel in storyImageHelperModels
                                    select storyImageHelperModel.Name);

                unitOfWork.StoryRepo.Delete(entity);

                try
                {
                    var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                    var fileResponse = await fileClient.DeleteStoryImagesAsync(imageNames).ConfigureAwait(false);
                    if (fileResponse.ErrorMessage.Count > 0)
                    {
                        if (rowsChanged > 0)
                            responseResult.ErrorMessage.Add("Story is deleted but unable to delete its all images!");
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
                    logger.LogError(ex, "Error deleting story: {Message}", msg);
                }
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteStoryImageAsync(int id, string imageName, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story not found for the deletion of image.");
                return responseResult;
            }
            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this story.");
                return responseResult;
            }

            var storyImageHelperModels = JsonConvert.DeserializeObject<List<StoryImageHelperModel>>(entity.StoryImage?.ImageNames ?? "[]");
            var storyImageHelperModel = storyImageHelperModels.SingleOrDefault(x => x.Name == imageName);
            if (storyImageHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Image not found to delete.");
                return responseResult;
            }
            storyImageHelperModels.Remove(storyImageHelperModel);

            var fileResponse = await fileClient.DeleteStoryImagesAsync([imageName]).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            if (storyImageHelperModels.Count == 0)
                entity.StoryImage = null;
            else
            {
                var imageNames = JsonConvert.SerializeObject(storyImageHelperModels);

                entity.StoryImage.ImageNames = imageNames;
                entity.UpdatedById = userId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<DataTableResponseDto<GetStoryDataTableDto>> GetAllAsync(StoryDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null)
        {
            var model = mapper.Map<StoryDataTableRequest>(modelDto);

            if (isAdmin.HasValue && !isAdmin.Value)
                model.StoryWorkerId = userId;
            var responseResult = new DataTableResponseDto<GetStoryDataTableDto>
            {
                AaData = mapper.Map<List<GetStoryDataTableDto>>(await unitOfWork.StoryRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.StoryRepo.GetTotalCountAsync().ConfigureAwait(false)
            };

            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);

            var userIds = responseResult.AaData.SelectMany(x => new List<string> { x.StoryWorkerId, x.PublishedById }).Distinct();
            var applicationUsers = await applicationUserService.GetUsersByUserIdsAsync(userIds).ConfigureAwait(false);

            foreach (var story in responseResult.AaData)
            {
                story.SubCategory = subCategories.SingleOrDefault(x => x.Id == story.SubCategoryId);
                story.StoryWorker = applicationUsers.SingleOrDefault(x => x.Id == story.StoryWorkerId);
                if (!string.IsNullOrEmpty(story.PublishedById))
                    story.PublishedBy = applicationUsers.SingleOrDefault(x => x.Id == story.PublishedById);
            }

            if (!string.IsNullOrEmpty(modelDto.Search?.Value) || !string.IsNullOrEmpty(modelDto.StoryWorkerId))
                responseResult.RecordsFiltered = await unitOfWork.StoryRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdateStoryDto>> GetForEditAsync(int id, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<GetUpdateStoryDto>();

            var entity = await unitOfWork.StoryRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Story not found by supplied Id.");
            else if (!isAdmin && entity.StoryWorkerId != userId)
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
            else
                responseResult.Data = mapper.Map<GetUpdateStoryDto>(entity);

            return responseResult;
        }

        public async Task<IEnumerable<GetStoryForListViewDto>> GetForListViewAsync(StoryDataTableForViewRequestDto modelDto)
        {
            var dtos = new List<GetStoryForListViewDto>();

            var model = mapper.Map<StoryDataTableForViewRequest>(modelDto);

            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);
            if (!string.IsNullOrEmpty(modelDto.SubCategoryName))
            {
                model.SubCategoryId = subCategories.FirstOrDefault(x => x.Name.Equals(modelDto.SubCategoryName, StringComparison.OrdinalIgnoreCase))?.Id;
                if (model.SubCategoryId == null)
                    return dtos;
            }

            var entities = await unitOfWork.StoryRepo.GetForListViewAsync(model).ConfigureAwait(false);

            foreach (var entity in entities)
            {
                var dto = new GetStoryForListViewDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    SubCategoryId = entity.SubCategoryId,
                    SubCategoryName = subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId).Name,
                    StoryLink = entity.StoryLink,
                    MainImageUrl = entity.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:StoryImageUrl")}/{entity.MainImage}",
                    PublishedOn = entity.PublishedOn
                };
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<ResponseMessageDto<GetStoryForViewDto>> GetForViewAsync(string subCategoryName, string storyLink)
        {
            var responseResult = new ResponseMessageDto<GetStoryForViewDto>();

            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);
            var subCategoryId = subCategories.Where(x => x.Name.Equals(subCategoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Id;

            if (!subCategoryId.HasValue)
            {
                responseResult.ErrorMessage.Add("Story not found by supplied subCategoryName.");
                return responseResult;
            }

            var entity = await unitOfWork.StoryRepo.GetForViewAsync(subCategoryId.Value, storyLink).ConfigureAwait(false);
            if (entity == null)
                responseResult.ErrorMessage.Add("Story not found by supplied storyLink.");
            else
            {
                responseResult.Data = mapper.Map<GetStoryForViewDto>(entity);
            }

            return responseResult;
        }

        public async Task<string> GetSitemapAsync()
        {
            var model = new StoryDataTableForViewRequest
            {
                Start = 0,
                Length = 15000
            };
            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);
            var entities = await unitOfWork.StoryRepo.GetForListViewAsync(model).ConfigureAwait(false);

            var sitemapData = new SitemapDto
            {
                Urls = new()
            };

            var webStories = CategoryConstant.WebStories.ToLower();
            foreach (var entity in entities)
            {
                sitemapData.Urls.Add(new UrlData
                {
                    LastMod = entity.UpdatedOn?.ToDateTimeUtcString(),
                    Loc = $"{config.GetValue<string>("SrcApiDotIn:SiteUrl")}/{webStories}/{subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId)?.Name}/{entity.StoryLink}".ToLower()
                });
            }
            sitemapData.Urls.Add(new UrlData
            {
                LastMod = DateTime.UtcNow.ToDateTimeUtcString(),
                Loc = $"{config.GetValue<string>("SrcApiDotIn:SiteUrl")}/{webStories}"
            });
            return sitemapData.SerializeToXml();
        }

        public async Task IncrementStoryViewForStoryIdsAsync(IEnumerable<int> storyIds)
        {
            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var groupedIds = storyIds.GroupBy(id => id)
                                        .Select(group => new { Id = group.Key, Count = group.Count() });
                var entities = await unitOfWork.StoryRepo.GetStoryViewWithLockByStoryIdsAsync(groupedIds.Select(x => x.Id)).ConfigureAwait(false);

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
                        unitOfWork.StoryRepo.AddStoryView(new()
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
                logger.LogError(ex, "Exception occurred while incrementing story views");
            }
        }

        public async Task<ResponseMessageDto<bool>> PublishStoryAsync(int storyId, PublishStoryDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to publish this story.");
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

            if (entity.IsPublished)
            {
                if (entity.MainImage == null)
                {
                    responseResult.ErrorMessage.Add("Please provide the main image of the story.");
                    return responseResult;
                }
                else if (string.IsNullOrEmpty(entity.Description))
                {
                    responseResult.ErrorMessage.Add("Please provide the description of the story.");
                    return responseResult;
                }
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

        public async Task<ResponseMessageDto<bool>> UpdateAsync(int storyId, UpdateStoryDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
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
                logger.LogError(ex, "Erroy updaing story: {Message}", msg);
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int storyId, string descriptionId, UpdateStoryDescriptionDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
                return responseResult;
            }

            var storyDescriptionHelperModels = JsonConvert.DeserializeObject<List<StoryDescriptionHelperModel>>(entity.Description ?? "[]");
            if (storyDescriptionHelperModels.Any(x => x.Priority == modelDto.Priority && x.Id != descriptionId))
            {
                responseResult.ErrorMessage.Add("Priority cannot be same.");
                return responseResult;
            }

            var storyDescriptionHelperModel = storyDescriptionHelperModels.SingleOrDefault(x => x.Id == descriptionId);
            if (storyDescriptionHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Description you want to update was not found.");
                return responseResult;
            }

            storyDescriptionHelperModel.Title = modelDto.Title;
            storyDescriptionHelperModel.Description = modelDto.Description;
            storyDescriptionHelperModel.Image = modelDto.Image;
            storyDescriptionHelperModel.ImageSource = modelDto.ImageSource;
            storyDescriptionHelperModel.Priority = modelDto.Priority;

            entity.Description = JsonConvert.SerializeObject(storyDescriptionHelperModels);

            modelDto.HtmlEncodeObject();
            entity.IpAddress = modelDto.IpAddress;
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateStoryImageAsync(int storyId, UpdateStoryImageDto modelDto, string userId, bool isAdmin)
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

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
                return responseResult;
            }

            var storyImageHelperModels = JsonConvert.DeserializeObject<List<StoryImageHelperModel>>(entity.StoryImage?.ImageNames ?? "[]");

            var fileResponse = await fileClient.UploadStoryImageAsync(modelDto.ImageFile).ConfigureAwait(false);
            if (fileResponse.ErrorMessage.Count > 0)
            {
                foreach (var error in fileResponse.ErrorMessage)
                {
                    responseResult.ErrorMessage.Add(error);
                }
                return responseResult;
            }

            storyImageHelperModels.Add(new StoryImageHelperModel
            {
                Name = fileResponse.Data,
                Source = modelDto.ImageSource
            });
            var imageNames = JsonConvert.SerializeObject(storyImageHelperModels);

            modelDto.HtmlEncodeObject();
            if (entity.StoryImage != null)
            {
                entity.StoryImage.ImageNames = imageNames;
                entity.StoryImage.IpAddress = modelDto.IpAddress;
                entity.StoryImage.UpdatedById = userId;
                entity.StoryImage.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.StoryImage = new()
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

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int storyId, UpdateStoryMainImageDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
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

        public async Task<ResponseMessageDto<bool>> UpdateStoryWorkerAsync(int storyId, UpdateStoryWorkerDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
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

        public async Task<ResponseMessageDto<bool>> AddDescriptionAsync(int storyId, UpdateStoryDescriptionDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
                return responseResult;
            }

            var storyDescriptionHelperModels = JsonConvert.DeserializeObject<List<StoryDescriptionHelperModel>>(entity.Description ?? "[]");
            if (storyDescriptionHelperModels.Any(x => x.Priority == modelDto.Priority))
            {
                responseResult.ErrorMessage.Add("Priority cannot be same.");
                return responseResult;
            }

            storyDescriptionHelperModels.Add(new StoryDescriptionHelperModel
            {
                Id = Guid.NewGuid().ToString(),
                Title = modelDto.Title,
                Description = modelDto.Description,
                Image = modelDto.Image,
                ImageSource = modelDto.ImageSource,
                Priority = modelDto.Priority
            });
            entity.Description = JsonConvert.SerializeObject(storyDescriptionHelperModels);

            modelDto.HtmlEncodeObject();
            entity.IpAddress = modelDto.IpAddress;
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteDescriptionAsync(int storyId, string descriptionId, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));
            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to delete description.");
                return responseResult;
            }
            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this story.");
                return responseResult;
            }

            var storyDescriptionHelperModels = JsonConvert.DeserializeObject<List<StoryDescriptionHelperModel>>(entity.Description ?? "[]");
            var storyDescriptionHelperModel = storyDescriptionHelperModels.SingleOrDefault(x => x.Id == descriptionId);
            if (storyDescriptionHelperModels == null)
            {
                responseResult.ErrorMessage.Add("Description not found to delete.");
                return responseResult;
            }
            storyDescriptionHelperModels.Remove(storyDescriptionHelperModel);

            entity.Description = JsonConvert.SerializeObject(storyDescriptionHelperModels);
            entity.UpdatedById = userId;
            entity.UpdatedOn = DateTime.UtcNow;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateStoryRelativeAsync(int storyId, UpdateStoryRelativeDto modelDto, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(storyId).ConfigureAwait(false);

            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story does not exist to update.");
                return responseResult;
            }

            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to update this story.");
                return responseResult;
            }

            var storyRelativeHelperModels = JsonConvert.DeserializeObject<List<StoryRelativeHelperModel>>(entity.StoryRelative?.RelativeUrl ?? "[]");

            var hrefLang = RelativeHrefLangConstant.GetHrefLangs()[modelDto.HrefLangId];
            if (string.IsNullOrEmpty(hrefLang))
            {
                responseResult.ErrorMessage.Add("HrefLang is not valid.");
                return responseResult;
            }
            else if (storyRelativeHelperModels.SingleOrDefault(x => x.HrefLang == hrefLang) != null)
            {
                responseResult.ErrorMessage.Add("HrefLang provided is already present.");
                return responseResult;
            }

            storyRelativeHelperModels.Add(new StoryRelativeHelperModel
            {
                HrefLang = hrefLang,
                Href = modelDto.Href
            });
            var relativeUrls = JsonConvert.SerializeObject(storyRelativeHelperModels);

            modelDto.HtmlEncodeObject();
            if (entity.StoryRelative != null)
            {
                entity.StoryRelative.RelativeUrl = relativeUrls;
                entity.StoryRelative.IpAddress = modelDto.IpAddress;
                entity.StoryRelative.UpdatedById = userId;
                entity.StoryRelative.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.StoryRelative = new StoryRelative
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

        public async Task<ResponseMessageDto<bool>> DeleteStoryRelativeAsync(int id, string hrefLang, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.StoryRepo.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Story not found for the deletion of relative.");
                return responseResult;
            }
            if (!isAdmin && entity.StoryWorkerId != userId)
            {
                responseResult.ErrorMessage.Add("Unauthorized to delete this story.");
                return responseResult;
            }

            var storyRelativeHelperModels = JsonConvert.DeserializeObject<List<StoryRelativeHelperModel>>(entity.StoryRelative?.RelativeUrl ?? "[]");
            var storyRelativeHelperModel = storyRelativeHelperModels.SingleOrDefault(x => x.HrefLang == hrefLang);
            if (storyRelativeHelperModel == null)
            {
                responseResult.ErrorMessage.Add("Relative Url not found to delete.");
                return responseResult;
            }
            storyRelativeHelperModels.Remove(storyRelativeHelperModel);

            if (storyRelativeHelperModels.Count == 0)
                entity.StoryRelative = null;
            else
            {
                var relativeUrls = JsonConvert.SerializeObject(storyRelativeHelperModels);

                entity.StoryRelative.RelativeUrl = relativeUrls;
                entity.UpdatedById = userId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task IncrementStoryView2ForStoryIdsAsync(Dictionary<int, int> data)
        {
            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var entities = await unitOfWork.StoryRepo.GetStoryViewWithLockByStoryIdsAsync(data.Select(x => x.Key)).ConfigureAwait(false);

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
                        unitOfWork.StoryRepo.AddStoryView(new()
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
                logger.LogError(ex, "Exception occurred while incrementing story views.");
            }
        }
    }
}
