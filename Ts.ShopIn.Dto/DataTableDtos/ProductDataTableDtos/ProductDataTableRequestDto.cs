using Ts.Dto.DataTableDtos;
namespace Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos
{
    public class ProductDataTableRequestDto : BaseDataTableRequestDto<ProductOrderDto>
    {
        public string ProductWorkerId { get; set; }
    }
}
