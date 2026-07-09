using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Client.HttpClientServices;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.PostVms;
using Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos;
using Ts.Dto;
namespace Ts.DotIn.Client.HttpClientServices.ClientServices
{
    public class PostForViewClient : IPostForViewClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpPublicService httpPublicService;
        private readonly ILogger<ContactClient> logger;
        private readonly IDistributedCache cache;
        private readonly string BaseUrl;
        private const string HomeCacheKey = "HomeDisplay";
        private const string PostCacheKey = "Post";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromMinutes(2);

        public PostForViewClient(IHttpPublicService httpPublicService, IConfiguration configuration, ILogger<ContactClient> logger,
            IDistributedCache cache)
        {
            this.httpPublicService = httpPublicService;
            this.logger = logger;
            this.cache = cache;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<IEnumerable<GetPostForListViewVm>>> DisplayItemsCacheAsync(PostDataTableForViewRequestDto model)
        {
            var cachedData = await cache.GetStringAsync(HomeCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<ResponseMessageDto<IEnumerable<GetPostForListViewVm>>>(cachedData);
            var response = await GetForListViewAsync(model).ConfigureAwait(false);

            if (response.ErrorMessage.Count == 0)
            {
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(_cacheExpiration)
                    .SetAbsoluteExpiration(_absoluteExpiration);
                await cache.SetStringAsync(HomeCacheKey, JsonConvert.SerializeObject(response), cacheEntryOptions).ConfigureAwait(false);
            }
            return response;
        }

        public async Task<ResponseMessageDto<IEnumerable<GetPostForListViewVm>>> GetForListViewAsync(PostDataTableForViewRequestDto model)
        {
            var response = await httpPublicService.PostAsync<IEnumerable<GetPostForListViewVm>>($"{BaseUrl}/{ApiUrl}/postdotin/post-list-for-view", model, true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching post list for view: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        private async Task<ResponseMessageDto<PostVm>> GetForViewAsync(string categoryName, string subCategoryName, string postLink)
        {
            var response = await httpPublicService.GetAsync<PostVm>($"{BaseUrl}/{ApiUrl}/postdotin/post-for-view/{categoryName}/{subCategoryName}/{postLink}", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching post for view: {ErrorMessages}", string.Join(',', response.ErrorMessage));

            if (response.ErrorMessage.Count == 0)
            {
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(_cacheExpiration)
                    .SetAbsoluteExpiration(_absoluteExpiration);
                await cache.SetStringAsync(PostCacheKey + "_" + categoryName + "_" + subCategoryName + "_" + postLink, JsonConvert.SerializeObject(response), cacheEntryOptions).ConfigureAwait(false);
            }
            return response;
        }

        public async Task<ResponseMessageDto<PostVm>> GetForViewCacheAsync(string categoryName, string subCategoryName, string postLink)
        {
            var cachedData = await cache.GetStringAsync(PostCacheKey + "_" + categoryName + "_" + subCategoryName + "_" + postLink).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<ResponseMessageDto<PostVm>>(cachedData);

            return await GetForViewAsync(categoryName, subCategoryName, postLink).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> GetSitemapAsync()
        {
            var response = await httpPublicService.GetAsync<string>($"{BaseUrl}/{ApiUrl}/postdotin/sitemap", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching sitemap: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        public async Task<ResponseMessageDto<bool>> UpdatePostForViewCountAsync(int postId, bool isBrowser)
        {
            if (isBrowser)
            {
                var response = await httpPublicService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/postdotin/post-for-view-count/{postId}", null, true).ConfigureAwait(false);
                if (response.ErrorMessage.Count > 0)
                    logger.LogError("Error updaing post view count: {ErrorMessages}", string.Join(',', response.ErrorMessage));
                return response;
            }
            return null;
        }
    }
}
