using Ts.Dto;
namespace Ts.DotCom.Service.HttpClientServices
{
    public interface IHttpFileClientService : IDisposable
    {
        Task<ResponseMessageDto<T>> PostMultipartAsync<T>(string url, MultipartFormDataContent content, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> PutAsync<T>(string url, object model, bool addAuthHeader = false);
    }
}
