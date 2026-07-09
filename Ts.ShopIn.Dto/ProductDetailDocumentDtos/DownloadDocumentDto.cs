namespace Ts.ShopIn.Dto.ProductDetailDocumentDtos
{
    public class DownloadDocumentDto
    {
        public string Token { get; set; }
        public string FileUrl { get; set; }
        public string EncryptedFileName { get; set; }
        public string AuthType { get; set; }
    }
}
