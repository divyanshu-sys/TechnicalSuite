using Ts.Dto;
namespace Ts.ShopIn.Service.HttpClientServices
{
    public interface IHttpFileClientService : IDisposable
    {
        string GetEncryptedFileToken(DateTime? dateUtcNow = null, string roleName = null);
        Task<ResponseMessageDto<T>> PostMultipartAsync<T>(string url, MultipartFormDataContent content, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> PutAsync<T>(string url, object model, bool addAuthHeader = false);
    }
}
