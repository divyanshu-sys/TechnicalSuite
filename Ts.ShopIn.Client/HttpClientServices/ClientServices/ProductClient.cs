using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Ts.Client.HttpClientServices;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Client.ViewModels.ProductImageVms;
using Ts.ShopIn.Client.ViewModels.ProductVms;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
using Ts.ShopIn.Dto.ProductDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class ProductClient : IProductClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public ProductClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/productdotin/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<ProductVm>>> GetAllAsync(ProductDataTableRequestVm model, bool isShowAll = false)
        {
            var productModel = mapper.Map<ProductDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<ProductOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 1)
                        sort.OrderBy.IsAvailable = true;
                    else if (order.Column == 2)
                        sort.OrderBy.IsPublished = true;
                    else if (order.Column == 3)
                        sort.OrderBy.PublishedOn = true;
                    else if (order.Column == 6)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 7)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    productModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<ProductVm>>($"{BaseUrl}/{ApiUrl}/productdotin/datatable/{isShowAll}", productModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<ProductVm>> GetForEditAsync(int id)
        {
            return await httpClientService.GetAsync<ProductVm>($"{BaseUrl}/{ApiUrl}/productdotin/{id}/for-edit", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateProductVm>> GetForUpdateProductAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateProductVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<UpdateProductMainImageVm>> GetForUpdateMainImageAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateProductMainImageVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<ProductVm>> PostAsync(CreateProductVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateProductDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<ProductVm>($"{BaseUrl}/{ApiUrl}/productdotin", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PublishProductAsync(int productId, bool isRepublish)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = new PublishProductDto
            {
                IsRepublish = isRepublish,
                IpAddress = ipAddress
            };
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdotin/{productId}/publish", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateProductVm model, int productId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateProductDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdotin/{productId}", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateProductMainImageVm model, int productId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateProductMainImageDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdotin/{productId}/main-image", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductImageAsync(UpdateProductImageVm model, int productId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            using var content = new MultipartFormDataContent();

            // Add the file content
            using var fileStream = model.ImageFile.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageFile.ContentType);
            content.Add(streamContent, "ImageFile", model.ImageFile.FileName);

            content.Add(new StringContent(ipAddress), "IpAddress");

            return await httpClientService.PutMultipartAsync<bool>($"{BaseUrl}/{ApiUrl}/productdotin/{productId}/product-image", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductImageAsync(int id, string imageName)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/productdotin/{id}/product-image/{imageName}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateProductWorkerVm>> GetForUpdateProductWorkerAsync(int id)
        {
            var vm = await GetForEditAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateProductWorkerVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductWorkerAsync(UpdateProductWorkerVm model, int productId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateProductWorkerDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/productdotin/{productId}/productworker", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetAllForDropDownAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<DropdownItemDto>>($"{BaseUrl}/{ApiUrl}/productdotin/dropdown", true).ConfigureAwait(false);
        }
    }
}
