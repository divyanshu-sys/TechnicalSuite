using Microsoft.AspNetCore.Http;
namespace Ts.Common.AppInterfaces
{
    public interface IFileValidationService
    {
        string ImageFileExtensionInValidMsg { get; }
        string ImageFileSizeInvalidMsg { get; }

        string DocumentFileExtensionInValidMsg { get; }
        string DocumentFileSizeInvalidMsg { get; }

        Task<bool> ValidateImageFileExtensionAsync(IFormFile imageFile);

        Task<bool> ValidateImageFileSizeAsync(IFormFile imageFile);

        Task<bool> ValidateDocumentFileExtensionAsync(IFormFile documentFile);

        Task<bool> ValidateDocumentFileSizeAsync(IFormFile documentFile);

        Task<bool> ValidateVideoFileExtensionAsync(IFormFile videoFile);

        Task<bool> ValidateVideoFileSizeAsync(IFormFile videoFile);
    }
}
