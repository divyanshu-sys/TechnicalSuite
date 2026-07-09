namespace Ts.ShopIn.Domain.DataTableModels.ProductDetailDataTables
{
    public class ProductDetailDataTableRequest : BaseDataTableRequest<ProductDetailOrder>
    {
        public string ProductDetailWorkerId { get; set; }
        public DateTime? LastViewedOnStart { get; set; }
        public DateTime? LastViewedOnEnd { get; set; }
    }
}
