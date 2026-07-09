namespace Ts.ShopIn.Domain.DataTableModels.ProductDataTables
{
    public class ProductOrder : BaseOrder
    {
        public bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public bool PublishedOn { get; set; }
    }
}
