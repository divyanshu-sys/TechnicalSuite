using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using Ts.Application.AppConstants;
using Ts.Application.Helpers;
using Ts.Common.Constant.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
namespace Ts.Service.HttpClientServices
{
    public class HttpFileClientService : IHttpFileClientService
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;
        private bool disposedValue;

        public HttpFileClientService(IConfiguration configuration, HttpClient httpClient)
        {
            this.configuration = configuration;
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            this.httpClient = httpClient;
        }

        public async Task<ResponseMessageDto<T>> PostMultipartAsync<T>(string url, MultipartFormDataContent content, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                await ConfigureAuthHeaderAsync();

            var responseMessage = await httpClient.PostAsync(url, content);
            return await ReadResponseAsync(responseMessage, response);
        }

        public async Task<ResponseMessageDto<T>> PutAsync<T>(string url, object model, bool addAuthHeader = false)
        {
            var response = new ResponseMessageDto<T>();
            if (addAuthHeader)
                await ConfigureAuthHeaderAsync();

            var responseMessage = await httpClient.PutAsync(url, model.ToJsonStringContent());
            return await ReadResponseAsync(responseMessage, response);
        }

        private Task ConfigureAuthHeaderAsync()
        {
            string encryptedToken = GetEncryptedFileToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encryptedToken);
            return Task.CompletedTask;
        }

        public string GetEncryptedFileToken(DateTime? dateUtcNow = null, string roleName = null)
        {
            var username = configuration.GetValue<string>("SrcApiDotIn:Username");
            var password = configuration.GetValue<string>("SrcApiDotIn:Password");
            var stringToken = $"{username};{password};{roleName ?? RoleConstant.Administrator};{dateUtcNow ?? DateTime.UtcNow.AddMinutes(1.5)}";
            var encryptedToken = AesEndeCryptor.EncryptString(EnDecryptionConstant.FileServiceKey, EnDecryptionConstant.FileServiceIv, stringToken);
            return encryptedToken;
        }

        private async Task<ResponseMessageDto<T>> ReadResponseAsync<T>(HttpResponseMessage httpResponseMessage, ResponseMessageDto<T> response)
        {
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                response.Data = !string.IsNullOrEmpty(responseData) ? JsonConvert.DeserializeObject<T>(responseData) : default;
            else if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                response.ErrorMessage.Add(!string.IsNullOrEmpty(responseData) ? responseData : httpResponseMessage.StatusCode.ToString());
            else if (httpResponseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                response.ErrorMessage = !string.IsNullOrEmpty(responseData) ? JsonConvert.DeserializeObject<List<string>>(responseData) : default;
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Created || httpResponseMessage.StatusCode == HttpStatusCode.Accepted || httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                response.Data = !string.IsNullOrEmpty(responseData) ? JsonConvert.DeserializeObject<T>(responseData) : true as dynamic;
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                response.ErrorMessage.Add("File Api! You are not authenticated.");
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
                response.ErrorMessage.Add("File Api! You are not authorised.");
            else
                response.ErrorMessage.Add(!string.IsNullOrEmpty(responseData) ? responseData : httpResponseMessage.StatusCode.ToString());

            return response;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    httpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
