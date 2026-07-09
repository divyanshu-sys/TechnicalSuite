namespace Ts.Domain.Models
{
    public class Address : BaseEntity
    {
        public new int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int PostOfficeId { get; set; }
        public virtual PostOffice PostOffice { get; set; }
    }
}
