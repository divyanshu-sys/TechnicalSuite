using Ts.ShopIn.Dto.ProductImageDtos;

namespace Ts.ShopIn.Dto.ProductDtos
{
    public class GetUpdateProductDto : ProductDto
    {
        public ProductImageDto ProductImage { get; set; }
    }
}
