using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Client.HttpClientServices;
using Ts.Dto;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogVms;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class BlogForViewClient : IBlogForViewClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpPublicService httpPublicService;
        private readonly ILogger<BlogForViewClient> logger;
        private readonly IDistributedCache cache;
        private readonly string BaseUrl;
        private const string BlogCacheKey = "Blog";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromMinutes(2);

        public BlogForViewClient(IHttpPublicService httpPublicService, IConfiguration configuration, ILogger<BlogForViewClient> logger,
            IDistributedCache cache)
        {
            this.httpPublicService = httpPublicService;
            this.logger = logger;
            this.cache = cache;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<IEnumerable<GetBlogForListViewVm>>> GetForListViewAsync(BlogDataTableForViewRequestDto model)
        {
            var response = await httpPublicService.PostAsync<IEnumerable<GetBlogForListViewVm>>($"{BaseUrl}/{ApiUrl}/blogdotin/blog-list-for-view", model, true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching blog for list view: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        private async Task<ResponseMessageDto<BlogVm>> GetForViewAsync(string subCategoryName, string blogLink)
        {
            var response = await httpPublicService.GetAsync<BlogVm>($"{BaseUrl}/{ApiUrl}/blogdotin/blog-for-view/{subCategoryName}/{blogLink}", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching blog for view: {ErrorMessages}", string.Join(',', response.ErrorMessage));

            if (response.ErrorMessage.Count == 0)
            {
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(_cacheExpiration)
                    .SetAbsoluteExpiration(_absoluteExpiration);
                await cache.SetStringAsync(BlogCacheKey + "_" + subCategoryName + "_" + blogLink, JsonConvert.SerializeObject(response), cacheEntryOptions).ConfigureAwait(false);
            }
            return response;
        }

        public async Task<ResponseMessageDto<BlogVm>> GetForViewCacheAsync(string subCategoryName, string blogLink)
        {
            var cachedData = await cache.GetStringAsync(BlogCacheKey + "_" + subCategoryName + "_" + blogLink).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<ResponseMessageDto<BlogVm>>(cachedData);

            return await GetForViewAsync(subCategoryName, blogLink).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> GetSitemapAsync()
        {
            var response = await httpPublicService.GetAsync<string>($"{BaseUrl}/{ApiUrl}/blogdotin/sitemap", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching sitemap: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        public async Task<ResponseMessageDto<bool>> UpdateBlogForViewCountAsync(int blogId, bool isBrowser)
        {
            if (isBrowser)
            {
                var response = await httpPublicService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/blogdotin/blog-for-view-count/{blogId}", null, true).ConfigureAwait(false);
                if (response.ErrorMessage.Count > 0)
                    logger.LogError("Error updating blog view count: {ErrorMessages}", string.Join(',', response.ErrorMessage));
                return response;
            }
            return null;
        }
    }
}
