namespace Ts.ShopIn.Domain.DataTableModels.ProductDataTables
{
    public class ProductDataTableForViewRequest
    {
        public string Search { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
