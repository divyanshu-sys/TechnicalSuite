namespace Ts.Domain.Models
{
    public class PaymentGatewayType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentStatus> PaymentStatuses { get; set; }
    }
}
