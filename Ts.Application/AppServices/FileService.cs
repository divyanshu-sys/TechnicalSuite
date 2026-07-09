using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;
using Ts.Application.AppConstants;
using Ts.Application.AppInterfaces;
using Ts.Application.Helpers;
using Ts.Application.Helpers.HelperClasses;
using Ts.Common.AppInterfaces;
using Ts.Common.HelperExtensions;
namespace Ts.Application.AppServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment env;
        private readonly IFileValidationService fileValidationService;

        public FileService(IWebHostEnvironment env, IFileValidationService fileValidationService,
            IConfiguration config)
        {
            this.env = env;
            this.fileValidationService = fileValidationService;
        }

        public async Task<FileStatus<string>> UploadByResizeImageAsync(ImageUploadData imageUploadData, string folderPath, bool isPublic = true, string oldFileName = null)
        {
            var fileStatus = new FileStatus<string>();
            if (!await fileValidationService.ValidateImageFileSizeAsync(imageUploadData.UploadFile).ConfigureAwait(false))
            {
                fileStatus.ErrorMessage.Add(fileValidationService.ImageFileSizeInvalidMsg);
                return fileStatus;
            }
            if (!await fileValidationService.ValidateImageFileExtensionAsync(imageUploadData.UploadFile).ConfigureAwait(false))
            {
                fileStatus.ErrorMessage.Add(fileValidationService.ImageFileExtensionInValidMsg);
                return fileStatus;
            }

            if (!string.IsNullOrEmpty(oldFileName))
                await DeleteOldFileAsync(folderPath, oldFileName, isPublic).ConfigureAwait(false);

            string uniqueName = GetUniqueFileName(imageUploadData.UploadFile.FileName);
            var filePath = await CreateFilePathAsync(folderPath, isPublic, uniqueName).ConfigureAwait(false);
            CreateFolderIfNotExist(filePath);

            using var image = Image.Load(imageUploadData.UploadFile.OpenReadStream());
            image.Mutate(x => x.Resize(imageUploadData.Width, imageUploadData.Height));
            image.Save(filePath);

            if (File.Exists(filePath))
                fileStatus.Data = uniqueName;
            return fileStatus;
        }

        public async Task<FileStatus<string>> UploadDocumentAsync(IFormFile file, string folderPath, bool isPublic = true, string oldFileName = null)
        {
            var fileStatus = new FileStatus<string>();
            if (!await fileValidationService.ValidateDocumentFileSizeAsync(file).ConfigureAwait(false))
            {
                fileStatus.ErrorMessage.Add(fileValidationService.ImageFileSizeInvalidMsg);
                return fileStatus;
            }
            if (!await fileValidationService.ValidateDocumentFileExtensionAsync(file).ConfigureAwait(false))
            {
                fileStatus.ErrorMessage.Add(fileValidationService.ImageFileExtensionInValidMsg);
                fileStatus.ErrorMessage.Add(fileValidationService.DocumentFileExtensionInValidMsg);
                return fileStatus;
            }

            if (!string.IsNullOrEmpty(oldFileName))
                await DeleteOldFileAsync(folderPath, oldFileName, isPublic).ConfigureAwait(false);

            fileStatus.Data = await CreateFileAsync(file, folderPath, isPublic).ConfigureAwait(false);
            return fileStatus;
        }

        public async Task<FileStatus<string>> UploadImageAsync(IFormFile file, string folderPath, bool isPublic = true, string oldFileName = null)
        {
            var fileStatus = new FileStatus<string>();
            if (!await fileValidationService.ValidateImageFileSizeAsync(file).ConfigureAwait(false))
            {
                fileStatus.ErrorMessage.Add(fileValidationService.ImageFileSizeInvalidMsg);
                return fileStatus;
            }
            if (!await fileValidationService.ValidateImageFileExtensionAsync(file).ConfigureAwait(false))
            {
                fileStatus.ErrorMessage.Add(fileValidationService.ImageFileExtensionInValidMsg);
                return fileStatus;
            }

            if (!string.IsNullOrEmpty(oldFileName))
                await DeleteOldFileAsync(folderPath, oldFileName, isPublic).ConfigureAwait(false);

            fileStatus.Data = await CreateFileAsync(file, folderPath, isPublic).ConfigureAwait(false);
            return fileStatus;
        }

        private Task<string> CreateFilePathAsync(string folderPath, bool isPublic, string fileName)
        {
            string filePath;
            if (isPublic)
                filePath = Path.Combine(Path.Combine(env.WebRootPath, folderPath), fileName);
            else
                filePath = Path.Combine(Path.Combine(env.ContentRootPath, folderPath), fileName);

            return Task.FromResult(filePath);
        }

        private static void CreateFolderIfNotExist(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        private static string GetUniqueFileName(string fileName)
        {
            var newFileName = Guid.NewGuid().ToString() + "_" + DateTime.UtcNow.ToDateIstString() + Path.GetExtension(fileName);
            return FileHelper.GetValidFileName(newFileName);
        }

        private async Task<string> CreateFileAsync(IFormFile file, string folderPath, bool isPublic)
        {
            string uniqueName = GetUniqueFileName(file.FileName);
            var filePath = await CreateFilePathAsync(folderPath, isPublic, uniqueName).ConfigureAwait(false);
            CreateFolderIfNotExist(filePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream).ConfigureAwait(false);
                await stream.DisposeAsync().ConfigureAwait(false);
            }
            if (File.Exists(filePath))
                return uniqueName;
            return null;
        }

        private async Task DeleteOldFileAsync(string folderPath, string oldFileName, bool isPublic)
        {
            var deleteFilePath = await CreateFilePathAsync(folderPath, isPublic, oldFileName).ConfigureAwait(false);

            if (File.Exists(deleteFilePath))
                File.Delete(deleteFilePath);
        }

        public async Task<FileStatus<bool>> DeleteFileAsync(string folderPath, IEnumerable<string> fileNames, bool isPublic = true)
        {
            var fileStatus = new FileStatus<bool>();

            foreach (var fileName in fileNames)
            {
                var deleteFilePath = await CreateFilePathAsync(folderPath, isPublic, fileName).ConfigureAwait(false);

                if (File.Exists(deleteFilePath))
                    File.Delete(deleteFilePath);
                else
                    fileStatus.ErrorMessage.Add("File not found to delete.");
            }

            if (fileStatus.ErrorMessage.Count > 0 && fileStatus.ErrorMessage.Count < fileNames.Count())
            {
                fileStatus.ErrorMessage.Add("Few File(s) are not found to delete.");
            }
            else
            {
                fileStatus.Data = true;
            }

            return fileStatus;
        }

        public Task ExtractZipFileIntoFolderAsync(IFormFile file, string folderPath, bool isPublic = true)
        {
            string uploadFolder;
            if (isPublic)
                uploadFolder = Path.Combine(env.WebRootPath, folderPath);
            else
                uploadFolder = Path.Combine(env.ContentRootPath, folderPath);

            using var archive = new ZipArchive(file.OpenReadStream());
            foreach (var entry in archive.Entries)
            {
                if (!string.IsNullOrEmpty(Path.GetExtension(entry.FullName)))
                    entry.ExtractToFile(Path.Combine(uploadFolder, entry.FullName), true);
                else
                    Directory.CreateDirectory(Path.Combine(uploadFolder, entry.FullName));
            }
            return Task.CompletedTask;
        }

        public async Task<FileStatus<byte[]>> GetFileByteArrayAsync(string filePath, bool isPublic = true)
        {
            var fileStatus = new FileStatus<byte[]>();

            string getFilePath;
            if (isPublic)
                getFilePath = Path.Combine(env.WebRootPath, filePath);
            else
                getFilePath = Path.Combine(env.ContentRootPath, filePath);

            if (File.Exists(getFilePath))
                fileStatus.Data = await File.ReadAllBytesAsync(getFilePath).ConfigureAwait(false);
            else
                fileStatus.ErrorMessage.Add("File does not exsit.");
            return fileStatus;
        }

        public FileStatus<FileStream> DownloadFile(string folderPath, string encryptedFileName)
        {
            var fileStatus = new FileStatus<FileStream>();

            try
            {
                var decodedFileName = AesEndeCryptor.DecryptString(EnDecryptionConstant.FileNameKey, EnDecryptionConstant.FileNameIv, encryptedFileName);

                var splitFileName = decodedFileName.Split(';');

                if (splitFileName.Length != 2 || Convert.ToDateTime(splitFileName[1]) < DateTime.UtcNow)
                {
                    fileStatus.ErrorMessage.Add("Not a valid file.");
                    return fileStatus;
                }

                var getFilePath = Path.Combine(env.ContentRootPath, $"{folderPath}/{splitFileName[0]}");
                if (File.Exists(getFilePath))
                {
                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(getFilePath, out string contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    fileStatus.ContentType = contentType;
                    fileStatus.FileName = Path.GetFileName(getFilePath);

                    fileStatus.Data = new FileStream(getFilePath, FileMode.Open, FileAccess.Read);
                }
                else
                    fileStatus.ErrorMessage.Add("File does not exsit.");
            }
            catch
            {
                fileStatus.ErrorMessage.Add("Invalid file.");
            }

            return fileStatus;
        }
    }
}
