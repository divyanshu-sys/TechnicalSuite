using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.RefreshTokenDtos;
namespace Ts.Client.HttpClientServices
{
    public class HttpClientService : IHttpClientService
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly string ApiBaseUri;

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HttpClient httpClient;

        public HttpClientService(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            this.httpContextAccessor = httpContextAccessor;
            ApiBaseUri = configuration["ApiBaseUri"];
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            this.httpClient = httpClient;
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

        public async Task<ResponseMessageDto<T>> PutAsync<T>(string url, object model, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                if (!await ConfigureAuthHeaderAsync().ConfigureAwait(false))
                {
                    response.ErrorMessage.Add("Token expired. Please try relogin.");
                    return response;
                }

            var responseMessage = await httpClient.PutAsync(url, model.ToJsonStringContent()).ConfigureAwait(false);
            return await ReadResponseAsync(responseMessage, response).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<T>> PutMultipartAsync<T>(string url, MultipartFormDataContent content, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                if (!await ConfigureAuthHeaderAsync().ConfigureAwait(false))
                {
                    response.ErrorMessage.Add("Token expired. Please try relogin.");
                    return response;
                }

            var responseMessage = await httpClient.PutAsync(url, content).ConfigureAwait(false);
            return await ReadResponseAsync(responseMessage, response).ConfigureAwait(false);
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

        public async Task<ResponseMessageDto<T>> DeleteAsync<T>(string url, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                if (!await ConfigureAuthHeaderAsync().ConfigureAwait(false))
                {
                    response.ErrorMessage.Add("Token expired. Please try relogin.");
                    return response;
                }

            var responseMessage = await httpClient.DeleteAsync(url).ConfigureAwait(false);
            return await ReadResponseAsync(responseMessage, response).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<T>> PatchAsync<T>(string url, object model, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                if (!await ConfigureAuthHeaderAsync().ConfigureAwait(false))
                {
                    response.ErrorMessage.Add("Token expired. Please try relogin.");
                    return response;
                }

            var responseMessage = await httpClient.PatchAsync(url, model.ToJsonStringContent()).ConfigureAwait(false);
            return await ReadResponseAsync(responseMessage, response).ConfigureAwait(false);
        }

        private async Task<bool> ConfigureAuthHeaderAsync()
        {
            var apiToken = httpContextAccessor.HttpContext.User.Claims.GetApiToken();

            // Api Token - Check for validity
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.ReadToken(apiToken) is JwtSecurityToken payload && payload.ValidTo < DateTime.UtcNow.AddSeconds(30)) // Token has expired
            {
                var refreshTokenExpiryAt = payload.Claims.First(x => x.Type == "refreshexpiryat").Value;
                var refreshTokenExpiryAtDate = string.IsNullOrEmpty(refreshTokenExpiryAt) ? (DateTime?)null : Convert.ToDateTime(refreshTokenExpiryAt);

                if (refreshTokenExpiryAtDate < DateTime.UtcNow.AddSeconds(30)) // RefreshToken has expired
                {
                    await httpContextAccessor.HttpContext.SignOutAsync().ConfigureAwait(false);
                    return false;
                }

                var httpResponseMessage = await httpClient.PostAsync($"{ApiBaseUri}/{ApiUrl}/authapplicationuser/refresh-token",
                    new AuthRefreshDto { EncodedRefreshToken = httpContextAccessor.HttpContext.User.Claims.GetRefreshToken() }
                    .ToJsonStringContent()).ConfigureAwait(false);
                var responseData = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    apiToken = !string.IsNullOrEmpty(responseData) ? JsonConvert.DeserializeObject<ApiTokenDto>(responseData)?.Token : default;

                    if (tokenHandler.ReadToken(apiToken) is JwtSecurityToken payloadNew)
                    {
                        var claimsIdentity = new ClaimsIdentity(payloadNew.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var browserIdentity = httpContextAccessor.HttpContext.User.Identities.First(x => x.AuthenticationType == "Browser Identity");

                        var tokenClaims = new List<Claim>
                        {
                            new("token", apiToken)
                        };
                        var tokenIdentity = new ClaimsIdentity(tokenClaims, "Token Identity");

                        var claimsPrincipal = new ClaimsPrincipal([claimsIdentity, browserIdentity, tokenIdentity]);

                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = httpContextAccessor.HttpContext.User.Claims.GetIsPersistent()
                        };

                        await httpContextAccessor.HttpContext.SignOutAsync().ConfigureAwait(false);
                        await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties).ConfigureAwait(false);
                    }
                }
                else
                {
                    await httpContextAccessor.HttpContext.SignOutAsync().ConfigureAwait(false);
                    return false;
                }
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpContextAccessor.HttpContext.User.Claims.GetAuthenticationScheme(), apiToken);
            return true;
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
                await httpContextAccessor.HttpContext.SignOutAsync().ConfigureAwait(false);
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
                response.ErrorMessage.Add("You are not authorised. Please try relogin.");
            else
            {
                response.ErrorMessage.Add(!string.IsNullOrEmpty(responseData) ? responseData : httpResponseMessage.StatusCode.ToString());
            }

            return response;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
