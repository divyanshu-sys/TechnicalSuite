using Ts.Dto;
namespace Ts.ShopIn.Client.HttpClientServices
{
    public interface IHttpClientUserService : IDisposable
    {
        Task<ResponseMessageDto<T>> PostAsync<T>(string url, object model, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> PutAsync<T>(string url, object model, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> GetAsync<T>(string url, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> DeleteAsync<T>(string url, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> PatchAsync<T>(string url, object model, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> PutMultipartAsync<T>(string url, MultipartFormDataContent content, bool addAuthHeader = false);
    }
}
