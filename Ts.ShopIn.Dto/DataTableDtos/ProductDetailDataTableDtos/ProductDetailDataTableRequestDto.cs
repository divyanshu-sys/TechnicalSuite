using Ts.Dto.DataTableDtos;
namespace Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos
{
    public class ProductDetailDataTableRequestDto : BaseDataTableRequestDto<ProductDetailOrderDto>
    {
        public string ProductDetailWorkerId { get; set; }
        public DateTimeOffset? LastViewedOnStart { get; set; }
        public DateTimeOffset? LastViewedOnEnd { get; set; }
    }
}
