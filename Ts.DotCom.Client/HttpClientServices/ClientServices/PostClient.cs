using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Ts.Client.HttpClientServices;
using Ts.Client.ViewModels.DataTableVms;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.DataTableVms;
using Ts.DotCom.Client.ViewModels.PostImageVms;
using Ts.DotCom.Client.ViewModels.PostRelativeVms;
using Ts.DotCom.Client.ViewModels.PostVms;
using Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotCom.Dto.PostDtos;
using Ts.DotCom.Dto.PostRelativeDtos;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
namespace Ts.DotCom.Client.HttpClientServices.ClientServices
{
    public class PostClient : IPostClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public PostClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<PostVm>>> GetAllAsync(PostDataTableRequestVm model, bool isShowAll = false, bool IsPagesVisited = false)
        {
            var postModel = mapper.Map<PostDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<PostOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (!IsPagesVisited)
                    {
                        if (order.Column == 4)
                            sort.OrderBy.IsPublished = true;
                        else if (order.Column == 5)
                            sort.OrderBy.PublishedOn = true;
                        else if (order.Column == 8)
                            sort.OrderBy.TotalViews = true;
                        else if (order.Column == 9)
                            sort.OrderBy.LastViewedOn = true;
                        else if (order.Column == 10)
                            sort.OrderBy.CreatedOn = true;
                        else if (order.Column == 11)
                            sort.OrderBy.UpdatedOn = true;
                    }
                    else
                    {
                        if (order.Column == 1)
                            sort.OrderBy.TotalViews = true;
                    }

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    postModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<PostVm>>($"{BaseUrl}/{ApiUrl}/postdotcom/datatable/{isShowAll}", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<PostVm>> GetForEditAsync(int id)
        {
            return await httpClientService.GetAsync<PostVm>($"{BaseUrl}/{ApiUrl}/postdotcom/{id}/for-edit", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdatePostVm>> GetForUpdatePostAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdatePostVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdatePostDescriptionVm>> GetForUpdateDescriptionAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdatePostDescriptionVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdatePostMainImageVm>> GetForUpdateMainImageAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdatePostMainImageVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<PostVm>> PostAsync(CreatePostVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreatePostDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<PostVm>($"{BaseUrl}/{ApiUrl}/postdotcom", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PublishPostAsync(int postId, bool isRepublish)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = new PublishPostDto
            {
                IsRepublish = isRepublish,
                IpAddress = ipAddress
            };
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{postId}/publish", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdatePostVm model, int postId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdatePostDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{postId}", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdatePostDescriptionVm model, int postId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdatePostDescriptionDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PatchAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{postId}/description", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdatePostMainImageVm model, int postId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdatePostMainImageDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{postId}/main-image", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdatePostImageAsync(UpdatePostImageVm model, int postId)
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

            return await httpClientService.PutMultipartAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{postId}/post-image", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeletePostImageAsync(int id, string imageName)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{id}/post-image/{imageName}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdatePostWorkerVm>> GetForUpdatePostWorkerAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdatePostWorkerVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<bool>> UpdatePostWorkerAsync(UpdatePostWorkerVm model, int postId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdatePostWorkerDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{postId}/postworker", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<int>> GetTotalUniquePagesVisited(string lastViewedOnStart = null, string lastViewedOnEnd = null)
        {
            var baseUrl = $"{BaseUrl}/{ApiUrl}/postdotcom/total-unique-pages-visited";

            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(lastViewedOnStart))
                queryParams["lastViewedOnStart"] = lastViewedOnStart;

            if (!string.IsNullOrWhiteSpace(lastViewedOnEnd))
                queryParams["lastViewedOnEnd"] = lastViewedOnEnd;

            var finalUrl = queryParams.Count > 0
                ? QueryHelpers.AddQueryString(baseUrl, queryParams)
                : baseUrl;

            return await httpClientService
                .GetAsync<int>(finalUrl, true)
                .ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<int>> GetTotalPagesVisitedLifetime()
        {
            return await httpClientService.GetAsync<int>($"{BaseUrl}/{ApiUrl}/postdotcom/total-pages-visited-lifetime", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdatePostRelativeAsync(UpdatePostRelativeVm model, int postId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

            var modelDto = mapper.Map<UpdatePostRelativeDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{postId}/post-relative", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeletePostRelativeAsync(int id, string hrefLang)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotcom/{id}/post-relative/{hrefLang}", true).ConfigureAwait(false);
        }
    }
}
