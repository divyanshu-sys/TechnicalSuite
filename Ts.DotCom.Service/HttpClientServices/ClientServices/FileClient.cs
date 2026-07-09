using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Ts.DotCom.Service.HttpClientServices.ClientInterfaces;
using Ts.Dto;
namespace Ts.DotCom.Service.HttpClientServices.ClientServices
{
    public class FileClient : IFileClient
    {
        private readonly IHttpFileClientService httpFileClientService;
        private readonly string UploadPostImageApiUrl;
        private readonly string DeletePostImageApiUrl;
        private readonly string UploadStoryImageApiUrl;
        private readonly string DeleteStoryImageApiUrl;

        public FileClient(IHttpFileClientService httpFileClientService, IConfiguration configuration)
        {
            this.httpFileClientService = httpFileClientService;
            UploadPostImageApiUrl = configuration.GetValue<string>("SrcApiDotCom:UploadPostImageApiUrl");
            DeletePostImageApiUrl = configuration.GetValue<string>("SrcApiDotCom:DeletePostImageApiUrl");

            UploadStoryImageApiUrl = configuration.GetValue<string>("SrcApiDotCom:UploadStoryImageApiUrl");
            DeleteStoryImageApiUrl = configuration.GetValue<string>("SrcApiDotCom:DeleteStoryImageApiUrl");
        }

        public async Task<ResponseMessageDto<bool>> DeletePostImagesAsync(IEnumerable<string> fileNames)
        {
            return await httpFileClientService.PutAsync<bool>($"{DeletePostImageApiUrl}", fileNames, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> UploadPostImageAsync(IFormFile file, string oldFileName = null)
        {
            using var content = new MultipartFormDataContent();

            // Add the file content
            using var fileStream = file.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, "file", file.FileName);
            if (!string.IsNullOrEmpty(oldFileName))
                content.Add(new StringContent(oldFileName), "oldFileName");

            return await httpFileClientService.PostMultipartAsync<string>($"{UploadPostImageApiUrl}", content, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteStoryImagesAsync(IEnumerable<string> fileNames)
        {
            return await httpFileClientService.PutAsync<bool>($"{DeleteStoryImageApiUrl}", fileNames, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<string>> UploadStoryImageAsync(IFormFile file, string oldFileName = null)
        {
            using var content = new MultipartFormDataContent();

            // Add the file content
            using var fileStream = file.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, "file", file.FileName);
            if (!string.IsNullOrEmpty(oldFileName))
                content.Add(new StringContent(oldFileName), "oldFileName");

            return await httpFileClientService.PostMultipartAsync<string>($"{UploadStoryImageApiUrl}", content, true).ConfigureAwait(false);
        }
    }
}
