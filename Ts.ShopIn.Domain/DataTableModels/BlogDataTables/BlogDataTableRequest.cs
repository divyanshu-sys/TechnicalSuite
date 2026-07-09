namespace Ts.ShopIn.Domain.DataTableModels.BlogDataTables
{
    public class BlogDataTableRequest : BaseDataTableRequest<BlogOrder>
    {
        public string BlogWorkerId { get; set; }
        public DateTime? LastViewedOnStart { get; set; }
        public DateTime? LastViewedOnEnd { get; set; }
    }
}
