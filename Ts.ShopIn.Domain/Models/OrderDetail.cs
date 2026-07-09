namespace Ts.ShopIn.Domain.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int OrderStatusId { get; set; }
        public int? ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public string Title { get; set; }
        public int ItemCount { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int ShopCategoryId { get; set; }
        public int ExchangePolicyId { get; set; }
        public int DeliveryPolicyId { get; set; }
        public int ReturnPolicyId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual OrderDetailStatusHistory OrderDetailStatusHistories { get; set; }
    }
}
