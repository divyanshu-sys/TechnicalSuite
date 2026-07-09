namespace Ts.DotCom.Domain.Models
{
    public abstract class BaseEntity
    {
        public object Id { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
        public byte[] ConcurrencyTimestamp { get; set; }
    }
}
