using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Ts.Application.AppConstants;
using Ts.Application.Helpers;
using Ts.Dto;
using Ts.ShopIn.Service.HttpClientServices.ClientInterfaces;
namespace Ts.ShopIn.Service.HttpClientServices.ClientServices
{
    public class FileClient : IFileClient
    {
        private readonly IHttpFileClientService httpFileClientService;
        private readonly string UploadBlogImageApiUrl;
        private readonly string DeleteBlogImageApiUrl;
        private readonly string UploadProductImageApiUrl;
        private readonly string DeleteProductImageApiUrl;
        private readonly string UploadProductDetailImageApiUrl;
        private readonly string DeleteProductDetailImageApiUrl;
        private readonly string UploadProductDetailDocumentApiUrl;
        private readonly string DeleteProductDetailDocumentApiUrl;

        public string DownloadDocumentUrl { get; }

        public FileClient(IHttpFileClientService httpFileClientService, IConfiguration configuration)
        {
            this.httpFileClientService = httpFileClientService;
            UploadBlogImageApiUrl = configuration.GetValue<string>("SrcApiShopIn:UploadBlogImageApiUrl");
            DeleteBlogImageApiUrl = configuration.GetValue<string>("SrcApiShopIn:DeleteBlogImageApiUrl");
            UploadProductImageApiUrl = configuration.GetValue<string>("SrcApiShopIn:UploadProductImageApiUrl");
            DeleteProductImageApiUrl = configuration.GetValue<string>("SrcApiShopIn:DeleteProductImageApiUrl");
            UploadProductDetailImageApiUrl = configuration.GetValue<string>("SrcApiShopIn:UploadProductDetailImageApiUrl");
            DeleteProductDetailImageApiUrl = configuration.GetValue<string>("SrcApiShopIn:DeleteProductDetailImageApiUrl");
            UploadProductDetailDocumentApiUrl = configuration.GetValue<string>("SrcApiShopIn:UploadProductDetailDocumentApiUrl");
            DeleteProductDetailDocumentApiUrl = configuration.GetValue<string>("SrcApiShopIn:DeleteProductDetailDocumentApiUrl");
            DownloadDocumentUrl = configuration.GetValue<string>("SrcApiShopIn:DownloadDocumentUrl");
        }

        public async Task<ResponseMessageDto<bool>> DeleteBlogImagesAsync(IEnumerable<string> fileNames)
        {
            return await httpFileClientService.PutAsync<bool>($"{DeleteBlogImageApiUrl}", fileNames, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> UploadBlogImageAsync(IFormFile file, string oldFileName = null)
        {
            using var content = new MultipartFormDataContent();

            // Add the file content
            using Stream fileStream = FileContent(file, oldFileName, content);

            return await httpFileClientService.PostMultipartAsync<string>($"{UploadBlogImageApiUrl}", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductImagesAsync(IEnumerable<string> fileNames)
        {
            return await httpFileClientService.PutAsync<bool>($"{DeleteProductImageApiUrl}", fileNames, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> UploadProductImageAsync(IFormFile file, string oldFileName = null)
        {
            using var content = new MultipartFormDataContent();

            // Add the file content
            using Stream fileStream = FileContent(file, oldFileName, content);

            return await httpFileClientService.PostMultipartAsync<string>($"{UploadProductImageApiUrl}", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductDetailImagesAsync(IEnumerable<string> fileNames)
        {
            return await httpFileClientService.PutAsync<bool>($"{DeleteProductDetailImageApiUrl}", fileNames, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> UploadProductDetailImageAsync(IFormFile file, string oldFileName = null)
        {
            using var content = new MultipartFormDataContent();

            // Add the file content
            using Stream fileStream = FileContent(file, oldFileName, content);

            return await httpFileClientService.PostMultipartAsync<string>($"{UploadProductDetailImageApiUrl}", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteProductDetailDocumentsAsync(IEnumerable<string> fileNames)
        {
            return await httpFileClientService.PutAsync<bool>($"{DeleteProductDetailDocumentApiUrl}", fileNames, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> UploadProductDetailDocumentAsync(IFormFile file, string oldFileName = null)
        {
            using var content = new MultipartFormDataContent();

            // Add the file content
            using Stream fileStream = FileContent(file, oldFileName, content);

            return await httpFileClientService.PostMultipartAsync<string>($"{UploadProductDetailDocumentApiUrl}", content, true).ConfigureAwait(false);
        }

        private static Stream FileContent(IFormFile file, string oldFileName, MultipartFormDataContent content)
        {
            var fileStream = file.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, "file", file.FileName);
            if (!string.IsNullOrEmpty(oldFileName))
                content.Add(new StringContent(oldFileName), "oldFileName");
            return fileStream;
        }

        public string GetEncryptedFileToken(DateTime dateUtcNow, string roleName)
        {
            return httpFileClientService.GetEncryptedFileToken(dateUtcNow.AddMinutes(1.5), roleName);
        }

        public string GetEncryptedFileName(string fileName, DateTime dateUtcNow)
        {
            var stringFileName = $"{fileName};{dateUtcNow.AddMinutes(1.5)}";
            var encryptedToken = AesEndeCryptor.EncryptString(EnDecryptionConstant.FileNameKey, EnDecryptionConstant.FileNameIv, stringFileName);
            return encryptedToken;
        }
    }
}
