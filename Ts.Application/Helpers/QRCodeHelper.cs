using ZXing.QrCode;
namespace Ts.Application.Helpers
{
    public static class QRCodeHelper
    {
        public static byte[] GenerateQR(string content, int height = 100, int width = 100, int margin = 0)
        {
            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = margin }
            };
            var pixelData = qrWriter.Write(content);
            // Create an image from the pixel data
            using var ms = new MemoryStream();
            // And save in ms variable
            var bitmapBytes = ms.ToArray();
            return bitmapBytes;
        }
    }
}
