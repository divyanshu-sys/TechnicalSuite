using Ts.Dto.DataTableDtos;
namespace Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos
{
    public class ProductDetailOrderDto : BaseOrderDto
    {
        public bool IsPublished { get; set; }
        public bool IsAvailable { get; set; }
        public bool Stock { get; set; }
        public bool Mrp { get; set; }
        public bool Price { get; set; }
        public bool PublishedOn { get; set; }
        public bool TotalViews { get; set; }
        public bool LastViewedOn { get; set; }
    }
}
