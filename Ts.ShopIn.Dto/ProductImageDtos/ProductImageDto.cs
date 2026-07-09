using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductImageDtos
{
    public class ProductImageDto : BaseDto
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public string ImageBaseUrl { get; set; }
    }
}
