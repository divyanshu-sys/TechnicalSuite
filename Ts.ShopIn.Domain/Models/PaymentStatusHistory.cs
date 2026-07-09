namespace Ts.ShopIn.Domain.Models
{
    public class PaymentStatusHistory
    {
        public int Id { get; set; }
        public virtual Payment Payment { get; set; }
        public string PaymentStatuses { get; set; } // Also add PaymentStatusAddedon in json

    }
}
