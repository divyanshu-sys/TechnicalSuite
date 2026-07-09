using Ts.Dto.DataTableDtos;
namespace Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos
{
    public class ProductOrderDto : BaseOrderDto
    {
        public bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
    }
}
