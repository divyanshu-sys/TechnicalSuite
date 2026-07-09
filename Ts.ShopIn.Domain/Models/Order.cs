namespace Ts.ShopIn.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ClientName { get; set; }
        public int PaymentModeId { get; set; }
        public int TotalItemCount { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public virtual ClientUser OrderedBy { get; set; }
        public DateTime OrderedOn { get; set; }

        public virtual Payment Payment { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
