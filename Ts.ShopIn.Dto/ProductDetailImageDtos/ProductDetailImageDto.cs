using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductDetailImageDtos
{
    public class ProductDetailImageDto : BaseDto
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public string ImageBaseUrl { get; set; }
    }
}
