namespace Ts.DotIn.Domain.DataTableModels.StoryDataTables
{
    public class StoryOrder : BaseOrder
    {
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
        public bool TotalViews { get; set; }
        public bool LastViewedOn { get; set; }
    }
}
