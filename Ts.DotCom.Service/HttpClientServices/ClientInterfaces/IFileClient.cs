using Microsoft.AspNetCore.Http;
using Ts.Dto;
namespace Ts.DotCom.Service.HttpClientServices.ClientInterfaces
{
    public interface IFileClient
    {
        Task<ResponseMessageDto<string>> UploadPostImageAsync(IFormFile file, string oldFileName = null);
        Task<ResponseMessageDto<bool>> DeletePostImagesAsync(IEnumerable<string> fileNames);

        Task<ResponseMessageDto<string>> UploadStoryImageAsync(IFormFile file, string oldFileName = null);
        Task<ResponseMessageDto<bool>> DeleteStoryImagesAsync(IEnumerable<string> fileNames);
    }
}
