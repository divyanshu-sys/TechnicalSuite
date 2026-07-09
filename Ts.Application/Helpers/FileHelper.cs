using System.IO.Compression;
using System.Text.RegularExpressions;
using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.Helpers
{
    public static class FileHelper
    {
        public static byte[] CreateArchive(IEnumerable<InMemoryFile> files)
        {
            using var archiveStream = new MemoryStream();
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                    using var zipStream = zipArchiveEntry.Open();
                    zipStream.Write(file.Content, 0, file.Content.Length);
                }
            }
            var archiveFile = archiveStream.ToArray();
            return archiveFile;
        }

        public static string GetValidFileName(string fileName)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars());
            var regex = new Regex($"[{Regex.Escape(regexSearch)}]");
            return regex.Replace(fileName, "");
        }
    }
}
