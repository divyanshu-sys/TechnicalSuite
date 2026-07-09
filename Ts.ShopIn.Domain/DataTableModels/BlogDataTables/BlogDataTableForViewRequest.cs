namespace Ts.ShopIn.Domain.DataTableModels.BlogDataTables
{
    public class BlogDataTableForViewRequest
    {
        public string Search { get; set; }
        public int? SubCategoryId { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
