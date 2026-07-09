using Microsoft.AspNetCore.Http;
using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.AppInterfaces
{
    public interface IFileService
    {
        Task<FileStatus<string>> UploadByResizeImageAsync(ImageUploadData imageUploadData, string folderPath, bool isPublic = true, string oldFileName = null);

        Task<FileStatus<string>> UploadImageAsync(IFormFile file, string folderPath, bool isPublic = true, string oldFileName = null);

        Task<FileStatus<string>> UploadDocumentAsync(IFormFile file, string folderPath, bool isPublic = true, string oldFileName = null);

        Task<FileStatus<bool>> DeleteFileAsync(string folderPath, IEnumerable<string> fileNames, bool isPublic = true);

        Task ExtractZipFileIntoFolderAsync(IFormFile file, string folderPath, bool isPublic = true);

        Task<FileStatus<byte[]>> GetFileByteArrayAsync(string filePath, bool isPublic = true);

        FileStatus<FileStream> DownloadFile(string folderPath, string encryptedFileName);
    }
}
