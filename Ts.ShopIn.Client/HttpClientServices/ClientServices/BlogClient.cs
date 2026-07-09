using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Ts.Client.HttpClientServices;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogImageVms;
using Ts.ShopIn.Client.ViewModels.BlogVms;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class BlogClient : IBlogClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public BlogClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<BlogVm>>> GetAllAsync(BlogDataTableRequestVm model, bool isShowAll = false, bool IsPagesVisited = false)
        {
            var blogModel = mapper.Map<BlogDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<BlogOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (!IsPagesVisited)
                    {
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
                    }
                    else
                    {
                        if (order.Column == 1)
                            sort.OrderBy.TotalViews = true;
                    }

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    blogModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<BlogVm>>($"{BaseUrl}/{ApiUrl}/blogdotin/datatable/{isShowAll}", blogModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<BlogVm>> GetForEditAsync(int id)
        {
            return await httpClientService.GetAsync<BlogVm>($"{BaseUrl}/{ApiUrl}/blogdotin/{id}/for-edit", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateBlogVm>> GetForUpdateBlogAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateBlogVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdateBlogDescriptionVm>> GetForUpdateDescriptionAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateBlogDescriptionVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdateBlogMainImageVm>> GetForUpdateMainImageAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateBlogMainImageVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<BlogVm>> PostAsync(CreateBlogVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateBlogDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<BlogVm>($"{BaseUrl}/{ApiUrl}/blogdotin", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PublishBlogAsync(int blogId, bool isRepublish)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = new PublishBlogDto
            {
                IsRepublish = isRepublish,
                IpAddress = ipAddress
            };
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{blogId}/publish", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateBlogVm model, int blogId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateBlogDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{blogId}", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdateBlogDescriptionVm model, int blogId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateBlogDescriptionDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PatchAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{blogId}/description", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateBlogMainImageVm model, int blogId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateBlogMainImageDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{blogId}/main-image", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateBlogImageAsync(UpdateBlogImageVm model, int blogId)
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

            return await httpClientService.PutMultipartAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{blogId}/blog-image", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteBlogImageAsync(int id, string imageName)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{id}/blog-image/{imageName}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateBlogWorkerVm>> GetForUpdateBlogWorkerAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateBlogWorkerVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<bool>> UpdateBlogWorkerAsync(UpdateBlogWorkerVm model, int blogId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateBlogWorkerDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/{blogId}/blogworker", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<int>> GetTotalUniquePagesVisited(string lastViewedOnStart = null, string lastViewedOnEnd = null)
        {
            var baseUrl = $"{BaseUrl}/{ApiUrl}/blogdotin/total-unique-pages-visited";

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
            return await httpClientService.GetAsync<int>($"{BaseUrl}/{ApiUrl}/blogdotin/total-pages-visited-lifetime", true).ConfigureAwait(false);
        }
    }
}
