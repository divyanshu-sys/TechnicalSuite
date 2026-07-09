namespace Ts.ShopIn.Domain.Models
{
    public class OrderDetailStatusHistory
    {
        public int Id { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
        public string OrderStatuses { get; set; } // Also add OrderStatusAddedon in json

    }
}
