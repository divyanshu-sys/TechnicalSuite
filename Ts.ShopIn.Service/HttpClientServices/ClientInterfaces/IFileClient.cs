using Microsoft.AspNetCore.Http;
using Ts.Dto;
namespace Ts.ShopIn.Service.HttpClientServices.ClientInterfaces
{
    public interface IFileClient
    {
        string DownloadDocumentUrl { get; }
        string GetEncryptedFileToken(DateTime dateUtcNow, string roleName);
        string GetEncryptedFileName(string fileName, DateTime dateUtcNow);

        Task<ResponseMessageDto<string>> UploadBlogImageAsync(IFormFile file, string oldFileName = null);
        Task<ResponseMessageDto<bool>> DeleteBlogImagesAsync(IEnumerable<string> fileNames);

        Task<ResponseMessageDto<string>> UploadProductImageAsync(IFormFile file, string oldFileName = null);
        Task<ResponseMessageDto<bool>> DeleteProductImagesAsync(IEnumerable<string> fileNames);

        Task<ResponseMessageDto<string>> UploadProductDetailImageAsync(IFormFile file, string oldFileName = null);
        Task<ResponseMessageDto<bool>> DeleteProductDetailImagesAsync(IEnumerable<string> fileNames);

        Task<ResponseMessageDto<string>> UploadProductDetailDocumentAsync(IFormFile file, string oldFileName = null);
        Task<ResponseMessageDto<bool>> DeleteProductDetailDocumentsAsync(IEnumerable<string> fileNames);
    }
}
