using Microsoft.AspNetCore.Http;
namespace Ts.Application.Helpers.HelperClasses
{
    public class ImageUploadData
    {
        public IFormFile UploadFile { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
