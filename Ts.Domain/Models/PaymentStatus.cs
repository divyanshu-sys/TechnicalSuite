namespace Ts.Domain.Models
{
    public class PaymentStatus
    {
        public int Id { get; set; }
        public int PaymentGatewayTypeId { get; set; }
        public virtual PaymentGatewayType PaymentGatewayType { get; set; }
        public string Name { get; set; }
    }
}
