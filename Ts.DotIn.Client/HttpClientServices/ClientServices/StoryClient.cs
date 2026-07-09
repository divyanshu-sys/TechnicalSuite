using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Ts.Client.HttpClientServices;
using Ts.Client.ViewModels.DataTableVms;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.DataTableVms;
using Ts.DotIn.Client.ViewModels.StoryImageVms;
using Ts.DotIn.Client.ViewModels.StoryRelativeVms;
using Ts.DotIn.Client.ViewModels.StoryVms;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotIn.Dto.StoryDtos;
using Ts.DotIn.Dto.StoryRelativeDtos;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
namespace Ts.DotIn.Client.HttpClientServices.ClientServices
{
    public class StoryClient : IStoryClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public StoryClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<StoryVm>>> GetAllAsync(StoryDataTableRequestVm model, bool isShowAll = false)
        {
            var storyModel = mapper.Map<StoryDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<StoryOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 3)
                        sort.OrderBy.IsPublished = true;
                    else if (order.Column == 4)
                        sort.OrderBy.PublishedOn = true;
                    else if (order.Column == 7)
                        sort.OrderBy.TotalViews = true;
                    else if (order.Column == 8)
                        sort.OrderBy.LastViewedOn = true;
                    else if (order.Column == 9)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 10)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    storyModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<StoryVm>>($"{BaseUrl}/{ApiUrl}/storydotin/datatable/{isShowAll}", storyModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<StoryVm>> GetForEditAsync(int id)
        {
            return await httpClientService.GetAsync<StoryVm>($"{BaseUrl}/{ApiUrl}/storydotin/{id}/for-edit", true).ConfigureAwait(false);
        }

        public UpdateStoryDescriptionVm GetForUpdateDescription(StoryVm vm, string descriptionId)
        {
            var description = vm.StoryDescriptionHelperVms.SingleOrDefault(x => x.Id == descriptionId);
            var updateVm = mapper.Map<UpdateStoryDescriptionVm>(description);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdateStoryVm>> GetForUpdateStoryAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateStoryVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdateStoryMainImageVm>> GetForUpdateMainImageAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateStoryMainImageVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<StoryVm>> PostAsync(CreateStoryVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateStoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<StoryVm>($"{BaseUrl}/{ApiUrl}/storydotin", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PublishStoryAsync(int storyId, bool isRepublish)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = new PublishStoryDto
            {
                IsRepublish = isRepublish,
                IpAddress = ipAddress
            };
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/publish", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateStoryVm model, int storyId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateStoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> AddDescriptionAsync(UpdateStoryDescriptionVm model, int storyId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateStoryDescriptionDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/description", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdateStoryDescriptionVm model, int storyId, string descriptionId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateStoryDescriptionDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/description/{descriptionId}", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteDescriptionAsync(int storyId, string descriptionId)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/description/{descriptionId}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateStoryMainImageVm model, int storyId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateStoryMainImageDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/main-image", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateStoryImageAsync(UpdateStoryImageVm model, int storyId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            using var content = new MultipartFormDataContent();

            // Add the file content
            using var fileStream = model.ImageFile.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageFile.ContentType);
            content.Add(streamContent, "ImageFile", model.ImageFile.FileName);

            // Add the other form data
            if (!string.IsNullOrEmpty(model.ImageSource))
                content.Add(new StringContent(model.ImageSource), "ImageSource");

            content.Add(new StringContent(ipAddress), "IpAddress");

            return await httpClientService.PutMultipartAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/story-image", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteStoryImageAsync(int id, string imageName)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{id}/story-image/{imageName}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateStoryWorkerVm>> GetForUpdateStoryWorkerAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateStoryWorkerVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<bool>> UpdateStoryWorkerAsync(UpdateStoryWorkerVm model, int storyId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateStoryWorkerDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/storyworker", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateStoryRelativeAsync(UpdateStoryRelativeVm model, int storyId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

            var modelDto = mapper.Map<UpdateStoryRelativeDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{storyId}/story-relative", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteStoryRelativeAsync(int id, string hrefLang)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/{id}/story-relative/{hrefLang}", true).ConfigureAwait(false);
        }
    }
}
