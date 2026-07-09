using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Client.HttpClientServices;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.StoryVms;
using Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.Dto;
namespace Ts.DotCom.Client.HttpClientServices.ClientServices
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
            var response = await httpPublicService.PostAsync<IEnumerable<GetStoryForListViewVm>>($"{BaseUrl}/{ApiUrl}/storydotcom/story-list-for-view", model, true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching story list for view: {ErrorMessage}", string.Join(',', response.ErrorMessage));
            return response;
        }

        private async Task<ResponseMessageDto<StoryVm>> GetForViewAsync(string subCategoryName, string storyLink)
        {
            var response = await httpPublicService.GetAsync<StoryVm>($"{BaseUrl}/{ApiUrl}/storydotcom/story-for-view/{subCategoryName}/{storyLink}", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching story for view: {ErrorMessage}", string.Join(',', response.ErrorMessage));

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
            var response = await httpPublicService.GetAsync<string>($"{BaseUrl}/{ApiUrl}/storydotcom/sitemap", true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching sitemap: {ErrorMessage}", string.Join(',', response.ErrorMessage));
            return response;
        }

        public async Task<ResponseMessageDto<bool>> UpdateStoryForViewCountAsync(int storyId, bool isBrowser)
        {
            if (isBrowser)
            {
                var response = await httpPublicService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/storydotcom/story-for-view-count/{storyId}", null, true).ConfigureAwait(false);
                if (response.ErrorMessage.Count > 0)
                    logger.LogError("Error updating story view count: {ErrorMessage}", string.Join(',', response.ErrorMessage));
                return response;
            }
            return null;
        }
    }
}
