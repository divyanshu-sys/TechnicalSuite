namespace Ts.ShopIn.Domain.Models
{
    public abstract class ClientBaseEntity
    {
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
        public byte[] ConcurrencyTimestamp { get; set; }
    }
}
