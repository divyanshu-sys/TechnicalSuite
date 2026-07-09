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
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailDocumentVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailImageVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class ProductDetailClient : IProductDetailClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public ProductDetailClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<ProductDetailVm>>> GetAllAsync(ProductDetailDataTableRequestVm model, bool isShowAll = false, bool IsPagesVisited = false)
        {
            var productdetailModel = mapper.Map<ProductDetailDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<ProductDetailOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (!IsPagesVisited)
                    {
                        if (order.Column == 3)
                            sort.OrderBy.IsAvailable = true;
                        if (order.Column == 4)
                            sort.OrderBy.IsPublished = true;
                        if (order.Column == 5)
                            sort.OrderBy.Stock = true;
                        if (order.Column == 6)
                            sort.OrderBy.Mrp = true;
                        if (order.Column == 7)
                            sort.OrderBy.Price = true;
                        else if (order.Column == 8)
                            sort.OrderBy.PublishedOn = true;
                        else if (order.Column == 11)
                            sort.OrderBy.TotalViews = true;
                        else if (order.Column == 12)
                            sort.OrderBy.LastViewedOn = true;
                        else if (order.Column == 13)
                            sort.OrderBy.CreatedOn = true;
                        else if (order.Column == 14)
                            sort.OrderBy.UpdatedOn = true;
                    }
                    else
                    {
                        if (order.Column == 1)
                            sort.OrderBy.TotalViews = true;
                    }

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    productdetailModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<ProductDetailVm>>($"{BaseUrl}/{ApiUrl}/productdetaildotin/datatable/{isShowAll}", productdetailModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<ProductDetailVm>> GetForEditAsync(int id)
        {
            return await httpClientService.GetAsync<ProductDetailVm>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{id}/for-edit", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateProductDetailVm>> GetForUpdateProductDetailAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateProductDetailVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdateProductDetailDescriptionVm>> GetForUpdateDescriptionAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateProductDetailDescriptionVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdateProductDetailMainImageVm>> GetForUpdateMainImageAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateProductDetailMainImageVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<ProductDetailVm>> PostAsync(CreateProductDetailVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateProductDetailDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<ProductDetailVm>($"{BaseUrl}/{ApiUrl}/productdetaildotin", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PublishProductDetailAsync(int productdetailId, bool isRepublish)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = new PublishProductDetailDto
            {
                IsRepublish = isRepublish,
                IpAddress = ipAddress
            };
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{productdetailId}/publish", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateProductDetailVm model, int productdetailId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateProductDetailDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{productdetailId}", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdateProductDetailDescriptionVm model, int productdetailId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateProductDetailDescriptionDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PatchAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{productdetailId}/description", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateProductDetailMainImageVm model, int productdetailId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateProductDetailMainImageDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{productdetailId}/main-image", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductDetailImageAsync(UpdateProductDetailImageVm model, int productdetailId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            using var content = new MultipartFormDataContent();

            // Add the file content
            using var fileStream = model.ImageFile.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageFile.ContentType);
            content.Add(streamContent, "ImageFile", model.ImageFile.FileName);

            content.Add(new StringContent(ipAddress), "IpAddress");

            return await httpClientService.PutMultipartAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{productdetailId}/productdetail-image", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductDetailImageAsync(int id, string imageName)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{id}/productdetail-image/{imageName}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductDetailDocumentAsync(UpdateProductDetailDocumentVm model, int productdetailId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            using var content = new MultipartFormDataContent();

            // Add the file content
            using var fileStream = model.DocumentFile.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.DocumentFile.ContentType);
            content.Add(streamContent, "DocumentFile", model.DocumentFile.FileName);

            content.Add(new StringContent(ipAddress), "IpAddress");

            return await httpClientService.PutMultipartAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{productdetailId}/productdetail-document", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductDetailDocumentAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{id}/productdetail-document", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateProductDetailWorkerVm>> GetForUpdateProductDetailWorkerAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateProductDetailWorkerVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductDetailWorkerAsync(UpdateProductDetailWorkerVm model, int productdetailId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateProductDetailWorkerDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{productdetailId}/productdetailworker", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<int>> GetTotalUniquePagesVisited(string lastViewedOnStart = null, string lastViewedOnEnd = null)
        {
            var baseUrl = $"{BaseUrl}/{ApiUrl}/productdetaildotin/total-unique-pages-visited";

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
            return await httpClientService.GetAsync<int>($"{BaseUrl}/{ApiUrl}/productdetaildotin/total-pages-visited-lifetime", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DownloadDocumentDto>> GetDocumentDetailAsync(int id)
        {
            return await httpClientService.GetAsync<DownloadDocumentDto>($"{BaseUrl}/{ApiUrl}/productdetaildotin/{id}/productdetail-document", true).ConfigureAwait(false);
        }
    }
}
