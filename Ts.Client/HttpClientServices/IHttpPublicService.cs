using Ts.Dto;
namespace Ts.Client.HttpClientServices
{
    public interface IHttpPublicService : IDisposable
    {
        Task<ResponseMessageDto<T>> PostAsync<T>(string url, object model, bool addAuthHeader = false);
        Task<ResponseMessageDto<T>> GetAsync<T>(string url, bool addAuthHeader = false);
    }
}
