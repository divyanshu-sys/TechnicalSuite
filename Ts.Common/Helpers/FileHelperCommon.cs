using System.Text;
using Ts.Common.Enums;
namespace Ts.Common.Helpers
{
    public static class FileHelperCommon
    {
        public static FileFormatEnum GetFileExtensionType(byte[] fileBytes)
        {
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg or jpg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon
            var png = new byte[] { 137, 80, 78, 71 }; // png
            var tiff = new byte[] { 73, 73, 42 }; // tiff
            var tiff2 = new byte[] { 77, 77, 42 }; // tiff2
            var officenew = new byte[] { 80, 75, 3, 4, 20 }; // OfficeNew
            var officeold = new byte[] { 208, 207, 17, 224, 161 }; // OfficeOld
            var corrupt = new byte[] { 77, 90, 144 };
            var bmp = Encoding.ASCII.GetBytes("BM"); // bmp
            var gif = Encoding.ASCII.GetBytes("GIF"); // gif
            var pdf = Encoding.ASCII.GetBytes("%PDF-"); // pdf

            var riff = Encoding.ASCII.GetBytes("RIFF"); // webp
            var webp = Encoding.ASCII.GetBytes("WEBP"); // webp

            if (jpeg.SequenceEqual(fileBytes.Take(jpeg.Length)))
                return FileFormatEnum.jpeg;
            else if (jpeg2.SequenceEqual(fileBytes.Take(jpeg2.Length)))
                return FileFormatEnum.jpeg2;
            else if (png.SequenceEqual(fileBytes.Take(png.Length)))
                return FileFormatEnum.png;
            else if (bmp.SequenceEqual(fileBytes.Take(bmp.Length)))
                return FileFormatEnum.bmp;
            else if (gif.SequenceEqual(fileBytes.Take(gif.Length)))
                return FileFormatEnum.gif;
            else if (tiff.SequenceEqual(fileBytes.Take(tiff.Length)))
                return FileFormatEnum.tiff;
            else if (tiff2.SequenceEqual(fileBytes.Take(tiff2.Length)))
                return FileFormatEnum.tiff2;
            else if (pdf.SequenceEqual(fileBytes.Take(pdf.Length)))
                return FileFormatEnum.pdf;
            else if (fileBytes.Length > 12 && riff.SequenceEqual(fileBytes.Take(riff.Length)) && webp.SequenceEqual(fileBytes.Skip(8).Take(webp.Length)))
                return FileFormatEnum.webp;
            else if (officenew.SequenceEqual(fileBytes.Take(officenew.Length)))
                return FileFormatEnum.officenew;
            else if (officeold.SequenceEqual(fileBytes.Take(officeold.Length)))
                return FileFormatEnum.officeold;
            else if (corrupt.SequenceEqual(fileBytes.Take(corrupt.Length)))
                return FileFormatEnum.corrupt;
            return FileFormatEnum.unknown;
        }
    }
}
