namespace Ts.ShopIn.Domain.DataTableModels.ProductDataTables
{
    public class ProductDataTableRequest : BaseDataTableRequest<ProductOrder>
    {
        public string ProductWorkerId { get; set; }
    }
}
