using Ts.Dto.DataTableDtos;
namespace Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos
{
    public class BlogOrderDto : BaseOrderDto
    {
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
        public bool TotalViews { get; set; }
        public bool LastViewedOn { get; set; }
    }
}
