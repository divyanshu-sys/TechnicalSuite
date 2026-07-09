namespace Ts.Domain.Models
{
    public class DeliveryPolicy
    {
        public int Id { get; set; }
        public int DeliveryInDays { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
