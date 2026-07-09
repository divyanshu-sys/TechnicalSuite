using Ts.Dto.DataTableDtos;
namespace Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos
{
    public class StoryOrderDto : BaseOrderDto
    {
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
        public bool TotalViews { get; set; }
        public bool LastViewedOn { get; set; }
    }
}
