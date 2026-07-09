namespace Ts.ShopIn.Domain.DataTableModels.BlogDataTables
{
    public class BlogOrder : BaseOrder
    {
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
        public bool TotalViews { get; set; }
        public bool LastViewedOn { get; set; }
    }
}
