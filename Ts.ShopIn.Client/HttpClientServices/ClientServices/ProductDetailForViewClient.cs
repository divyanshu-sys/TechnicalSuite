using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Client.HttpClientServices;
using Ts.Dto;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class ProductDetailForViewClient : IProductDetailForViewClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpPublicService httpPublicService;
        private readonly ILogger<ProductDetailForViewClient> logger;
        private readonly IDistributedCache cache;
        private readonly string BaseUrl;
        private const string ProductDetailCacheKey = "ProductDetail";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromMinutes(2);

        public ProductDetailForViewClient(IHttpPublicService httpPublicService, IConfiguration configuration,
            ILogger<ProductDetailForViewClient> logger, IDistributedCache cache)
        {
            this.httpPublicService = httpPublicService;
            this.logger = logger;
            this.cache = cache;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<IEnumerable<GetProductDetailForListViewVm>>> GetForListViewAsync(ProductDetailDataTableForViewRequestDto model)
        {
            var response = await httpPublicService.PostAsync<IEnumerable<GetProductDetailForListViewVm>>($"{BaseUrl}/{ApiUrl}/productdetaildotin/productdetail-list-for-view", model, true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching product detail for list view: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        private async Task<ResponseMessageDto<ProductDetailVm>> GetForViewAsync(string shopCategoryName, string productdetailLink)
        {
            var response = await httpPublicService.GetAsync<ProductDetailVm>($"{BaseUrl}/{ApiUrl}/productdetaildotin/productdetail-for-view/{shopCategoryName}/{productdetailLink}", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching product detail for view: {ErrorMessages}", string.Join(',', response.ErrorMessage));

            if (response.ErrorMessage.Count == 0)
            {
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(_cacheExpiration)
                    .SetAbsoluteExpiration(_absoluteExpiration);
                await cache.SetStringAsync(ProductDetailCacheKey + "_" + shopCategoryName + "_" + productdetailLink, JsonConvert.SerializeObject(response), cacheEntryOptions).ConfigureAwait(false);
            }
            return response;
        }

        public async Task<ResponseMessageDto<ProductDetailVm>> GetForViewCacheAsync(string shopCategoryName, string productdetailLink)
        {
            var cachedData = await cache.GetStringAsync(ProductDetailCacheKey + "_" + shopCategoryName + "_" + productdetailLink).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<ResponseMessageDto<ProductDetailVm>>(cachedData);

            return await GetForViewAsync(shopCategoryName, productdetailLink).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> GetSitemapAsync()
        {
            var response = await httpPublicService.GetAsync<string>($"{BaseUrl}/{ApiUrl}/productdetaildotin/sitemap", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching sitemap: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        public async Task<ResponseMessageDto<bool>> UpdateProductDetailForViewCountAsync(int productdetailId, bool isBrowser)
        {
            if (isBrowser)
            {
                var response = await httpPublicService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/productdetaildotin/productdetail-for-view-count/{productdetailId}", null, true).ConfigureAwait(false);
                if (response.ErrorMessage.Count > 0)
                    logger.LogError("Error updating product detail view count: {ErrorMessages}", string.Join(',', response.ErrorMessage));
                return response;
            }
            return null;
        }
    }
}
