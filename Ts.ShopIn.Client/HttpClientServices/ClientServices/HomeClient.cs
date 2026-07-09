using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Client.HttpClientServices;
using Ts.Dto;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.HomeVms;

namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class HomeClient : IHomeClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpPublicService httpPublicService;
        private readonly ILogger<HomeClient> logger;
        private readonly IDistributedCache cache;
        private readonly string BaseUrl;
        private const string HomeCacheKey = "HomeDisplay";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromMinutes(2);

        public HomeClient(IHttpPublicService httpPublicService, IConfiguration configuration, ILogger<HomeClient> logger,
            IDistributedCache cache)
        {
            this.httpPublicService = httpPublicService;
            this.logger = logger;
            this.cache = cache;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<DisplayHomeItemsVm>> DisplayItemsCacheAsync()
        {
            var cachedData = await cache.GetStringAsync(HomeCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<ResponseMessageDto<DisplayHomeItemsVm>>(cachedData);
            return await DisplayItemsAsync().ConfigureAwait(false);
        }

        private async Task<ResponseMessageDto<DisplayHomeItemsVm>> DisplayItemsAsync()
        {
            var response = await httpPublicService.GetAsync<DisplayHomeItemsVm>($"{BaseUrl}/{ApiUrl}/shopinapp/home-list-for-view", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching display items: {ErrorMessages}", string.Join(',', response.ErrorMessage));

            if (response.ErrorMessage.Count == 0)
            {
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(_cacheExpiration)
                    .SetAbsoluteExpiration(_absoluteExpiration);
                await cache.SetStringAsync(HomeCacheKey, JsonConvert.SerializeObject(response), cacheEntryOptions).ConfigureAwait(false);
            }
            return response;
        }
    }
}
