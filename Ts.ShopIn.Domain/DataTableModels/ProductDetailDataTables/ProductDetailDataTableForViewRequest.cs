namespace Ts.ShopIn.Domain.DataTableModels.ProductDetailDataTables
{
    public class ProductDetailDataTableForViewRequest
    {
        public string Search { get; set; }
        public int? ShopCategoryId { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
