using Ts.ShopIn.Dto.ProductDetailDtos;

namespace Ts.ShopIn.Dto.CartDtos
{
    public class GetCartForViewDto : CartDto
    {
        public decimal TotalPrice { get; set; }
        public GetProductDetailForListViewDto ProductDetail { get; set; }
    }
}
