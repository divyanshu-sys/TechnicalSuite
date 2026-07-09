using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.RefreshTokenDtos;
namespace Ts.Client.HttpClientServices
{
    public class HttpPublicService : IHttpPublicService
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly string BaseUri;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HttpClient httpClient;
        private readonly IMemoryCache cache;

        private const string AuthCacheKey = "AuthToken";

        public HttpPublicService(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, HttpClient httpClient,
            IMemoryCache cache)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            BaseUri = configuration["Api:BaseUri"];
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            this.httpClient = httpClient;
            this.cache = cache;
        }

        public async Task<ResponseMessageDto<T>> GetAsync<T>(string url, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                if (!await ConfigureAuthHeaderAsync().ConfigureAwait(false))
                {
                    response.ErrorMessage.Add("Token expired. Please try relogin.");
                    return response;
                }

            var responseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);
            return await ReadResponseAsync(responseMessage, response).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<T>> PostAsync<T>(string url, object model, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                if (!await ConfigureAuthHeaderAsync().ConfigureAwait(false))
                {
                    response.ErrorMessage.Add("Token expired. Please try relogin.");
                    return response;
                }

            var responseMessage = await httpClient.PostAsync(url, model.ToJsonStringContent()).ConfigureAwait(false);
            return await ReadResponseAsync(responseMessage, response).ConfigureAwait(false);
        }

        private async Task<bool> ConfigureAuthHeaderAsync()
        {
            if (!cache.TryGetValue(AuthCacheKey, out AuthTokenDto authTokenDto))
            {
                authTokenDto = await SetAuthTokenAsync().ConfigureAwait(false);
            }

            if (authTokenDto == null) return false;

            // Api Token - Check for validity
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.ReadToken(authTokenDto.ApiToken) is JwtSecurityToken payload && payload.ValidTo < DateTime.UtcNow.AddSeconds(30)) // Token has expired
            {
                var refreshTokenExpiryAt = payload.Claims.First(x => x.Type == "refreshexpiryat").Value;
                var refreshTokenExpiryAtDate = string.IsNullOrEmpty(refreshTokenExpiryAt) ? (DateTime?)null : Convert.ToDateTime(refreshTokenExpiryAt);

                if (refreshTokenExpiryAtDate < DateTime.UtcNow.AddSeconds(30)) // RefreshToken has expired
                {
                    authTokenDto = await SetAuthTokenAsync().ConfigureAwait(false);
                    if (authTokenDto == null) return false;
                }
                else
                {
                    var refreshHttpResponseMessage = await httpClient.PostAsync($"{BaseUri}/{ApiUrl}/authapplicationuser/refresh-token",
                        new AuthRefreshDto { EncodedRefreshToken = authTokenDto.RefreshEncodedToken }
                        .ToJsonStringContent()).ConfigureAwait(false);
                    var refreshResponseData = await refreshHttpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (refreshHttpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        var newApiToken = !string.IsNullOrEmpty(refreshResponseData) ? JsonConvert.DeserializeObject<ApiTokenDto>(refreshResponseData)?.Token : default;
                        if (string.IsNullOrEmpty(newApiToken)) return false;
                        authTokenDto.ApiToken = newApiToken;
                        CacheAuthToken(authTokenDto);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authTokenDto.AuthenticationScheme, authTokenDto.ApiToken);
            return true;
        }

        private async Task<AuthTokenDto> SetAuthTokenAsync()
        {
            var modelDto = new LoginDto
            {
                Email = configuration["Api:Username"],
                Password = configuration["Api:Password"],
                IpAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress)
            };

            var authHttpResponseMessage = await httpClient.PostAsync($"{BaseUri}/{ApiUrl}/authapplicationuser", modelDto.ToJsonStringContent()).ConfigureAwait(false);
            var authResponseData = await authHttpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (authHttpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var authTokenDto = !string.IsNullOrEmpty(authResponseData) ? JsonConvert.DeserializeObject<AuthTokenDto>(authResponseData) : default;

                if (authTokenDto != null)
                    CacheAuthToken(authTokenDto);

                return authTokenDto;
            }
            return null;
        }

        private void CacheAuthToken(AuthTokenDto authTokenDto)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(Convert.ToDouble(configuration["Api:CacheTokenExpiresInMinute"])));

            cache.Set(AuthCacheKey, authTokenDto, cacheEntryOptions);
        }

        private async Task<ResponseMessageDto<T>> ReadResponseAsync<T>(HttpResponseMessage httpResponseMessage, ResponseMessageDto<T> response)
        {
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                response.Data = !string.IsNullOrEmpty(responseData) ? JsonConvert.DeserializeObject<T>(responseData) : default;
            else if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                response.ErrorMessage.Add(!string.IsNullOrEmpty(responseData) ? responseData : httpResponseMessage.StatusCode.ToString());
            else if (httpResponseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                response.ErrorMessage = !string.IsNullOrEmpty(responseData) ? JsonConvert.DeserializeObject<List<string>>(responseData) : default;
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Created || httpResponseMessage.StatusCode == HttpStatusCode.Accepted || httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                response.Data = !string.IsNullOrEmpty(responseData) ? JsonConvert.DeserializeObject<T>(responseData) : true as dynamic;
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.ErrorMessage.Add("You are not authenticated. Please try relogin.");
                cache.Remove(AuthCacheKey);
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                response.ErrorMessage.Add("You are not authorised. Please try relogin.");
                cache.Remove(AuthCacheKey);
            }
            else
                response.ErrorMessage.Add(!string.IsNullOrEmpty(responseData) ? responseData : httpResponseMessage.StatusCode.ToString());

            return response;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
