using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;
using Ts.ShopIn.Dto.ProductDtos;

namespace Ts.ShopIn.Dto.HomeDtos
{
    public class DisplayHomeItemsDto
    {
        public List<GetBlogForListViewDto> BlogForListView { get; set; }
        public List<GetProductDetailForListViewDto> ProductDetailForListView { get; set; }
        public List<GetProductForListViewDto> ProductForListView { get; set; }
    }
}
