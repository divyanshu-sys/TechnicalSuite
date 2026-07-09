using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Common.AppInterfaces;
using Ts.Common.Helpers;
namespace Ts.Common.AppServices
{
    public class FileValidationService : IFileValidationService
    {
        private readonly IEnumerable<string> _allowedImageFileExtensions;
        private readonly IEnumerable<string> _allowedVideoFileExtensions;
        private readonly IEnumerable<string> _allowedDocumentFileExtensions;
        private readonly IEnumerable<string> _allowedImageContentTypes;
        private readonly int _allowedImageFileSize;
        private readonly int _allowedDocumentFileSize;
        private readonly int _allowedVideoFileSize;

        public FileValidationService(IConfiguration config)
        {
            _allowedDocumentFileExtensions = config.GetSection("AllowedDocumentFileExtensions").Get<List<string>>();
            _allowedImageFileExtensions = config.GetSection("AllowedImageFileExtensions").Get<List<string>>();
            _allowedVideoFileExtensions = config.GetSection("AllowedVideoFileExtensions").Get<List<string>>();
            _allowedDocumentFileSize = config.GetValue<int>("AllowedDocumentFileSize");
            _allowedImageFileSize = config.GetValue<int>("AllowedImageFileSize");
            _allowedVideoFileSize = config.GetValue<int>("AllowedVideoFileSize");
            _allowedImageContentTypes = config.GetSection("AllowedImageContentTypes").Get<List<string>>();

            ImageFileExtensionInValidMsg = $"Only {string.Join(", ", _allowedImageFileExtensions)} are allowed.";
            ImageFileSizeInvalidMsg = $"File size should be less than {_allowedImageFileSize / 1024F} KB.";

            DocumentFileExtensionInValidMsg = $"Only {string.Join(", ", _allowedDocumentFileExtensions)} are allowed.";
            DocumentFileSizeInvalidMsg = $"File size should be less than {_allowedDocumentFileSize / 1048576F} MB.";
        }

        public string ImageFileExtensionInValidMsg { get; }
        public string ImageFileSizeInvalidMsg { get; }

        public string DocumentFileExtensionInValidMsg { get; }
        public string DocumentFileSizeInvalidMsg { get; }

        public Task<bool> ValidateImageFileExtensionAsync(IFormFile imageFile)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                imageFile.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            var fileType = FileHelperCommon.GetFileExtensionType(fileBytes);
            return Task.FromResult(_allowedImageFileExtensions.Contains(fileType.ToString()) && _allowedImageContentTypes.Contains(imageFile.ContentType));
        }

        public Task<bool> ValidateImageFileSizeAsync(IFormFile imageFile)
        {
            return Task.FromResult(imageFile.Length <= _allowedImageFileSize);
        }

        public Task<bool> ValidateDocumentFileExtensionAsync(IFormFile documentFile)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                documentFile.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            var fileType = FileHelperCommon.GetFileExtensionType(fileBytes);
            return Task.FromResult(_allowedDocumentFileExtensions.Contains(fileType.ToString()) ||
                (_allowedImageFileExtensions.Contains(fileType.ToString()) && _allowedImageContentTypes.Contains(documentFile.ContentType)));
        }

        public Task<bool> ValidateDocumentFileSizeAsync(IFormFile documentFile)
        {
            return Task.FromResult(documentFile.Length <= _allowedDocumentFileSize);
        }

        public Task<bool> ValidateVideoFileExtensionAsync(IFormFile videoFile)
        {
            var extention = Path.GetExtension(videoFile.FileName);
            return Task.FromResult(_allowedVideoFileExtensions.Contains(extention));
        }

        public Task<bool> ValidateVideoFileSizeAsync(IFormFile videoFile)
        {
            return Task.FromResult(videoFile.Length <= _allowedVideoFileSize);
        }
    }
}
