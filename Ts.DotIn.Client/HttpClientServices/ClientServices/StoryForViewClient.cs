using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Client.HttpClientServices;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.StoryVms;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.Dto;
namespace Ts.DotIn.Client.HttpClientServices.ClientServices
{
    public class StoryForViewClient : IStoryForViewClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpPublicService httpPublicService;
        private readonly ILogger<ContactClient> logger;
        private readonly IDistributedCache cache;
        private readonly string BaseUrl;
        private const string StoryCacheKey = "Story";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromMinutes(2);

        public StoryForViewClient(IHttpPublicService httpPublicService, IConfiguration configuration, ILogger<ContactClient> logger,
            IDistributedCache cache)
        {
            this.httpPublicService = httpPublicService;
            this.logger = logger;
            this.cache = cache;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<IEnumerable<GetStoryForListViewVm>>> GetForListViewAsync(StoryDataTableForViewRequestDto model)
        {
            var response = await httpPublicService.PostAsync<IEnumerable<GetStoryForListViewVm>>($"{BaseUrl}/{ApiUrl}/storydotin/story-list-for-view", model, true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching story for list view: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        private async Task<ResponseMessageDto<StoryVm>> GetForViewAsync(string subCategoryName, string storyLink)
        {
            var response = await httpPublicService.GetAsync<StoryVm>($"{BaseUrl}/{ApiUrl}/storydotin/story-for-view/{subCategoryName}/{storyLink}", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Errot fetching story for view: {ErrorMessages}", string.Join(',', response.ErrorMessage));

            if (response.ErrorMessage.Count == 0)
            {
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(_cacheExpiration)
                    .SetAbsoluteExpiration(_absoluteExpiration);
                await cache.SetStringAsync(StoryCacheKey + "_" + subCategoryName + "_" + storyLink, JsonConvert.SerializeObject(response), cacheEntryOptions).ConfigureAwait(false);
            }
            return response;
        }

        public async Task<ResponseMessageDto<StoryVm>> GetForViewCacheAsync(string subCategoryName, string storyLink)
        {
            var cachedData = await cache.GetStringAsync(StoryCacheKey + "_" + subCategoryName + "_" + storyLink).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<ResponseMessageDto<StoryVm>>(cachedData);

            return await GetForViewAsync(subCategoryName, storyLink).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> GetSitemapAsync()
        {
            var response = await httpPublicService.GetAsync<string>($"{BaseUrl}/{ApiUrl}/storydotin/sitemap", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching sitemap: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }

        public async Task<ResponseMessageDto<bool>> UpdateStoryForViewCountAsync(int storyId, bool isBrowser)
        {
            if (isBrowser)
            {
                var response = await httpPublicService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotin/story-for-view-count/{storyId}", null, true).ConfigureAwait(false);
                if (response.ErrorMessage.Count > 0)
                    logger.LogError("Error updating story view count: {ErrorMessages}", string.Join(',', response.ErrorMessage));
                return response;
            }
            return null;
        }
    }
}
