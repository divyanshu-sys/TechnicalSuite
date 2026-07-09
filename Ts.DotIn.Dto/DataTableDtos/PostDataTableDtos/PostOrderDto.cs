using Ts.Dto.DataTableDtos;
namespace Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos
{
    public class PostOrderDto : BaseOrderDto
    {
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
        public bool TotalViews { get; set; }
        public bool LastViewedOn { get; set; }
    }
}
