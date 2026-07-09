namespace Ts.DotCom.Domain.DataTableModels.PostDataTables
{
    public class PostOrder : BaseOrder
    {
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
        public bool TotalViews { get; set; }
        public bool LastViewedOn { get; set; }
    }
}
