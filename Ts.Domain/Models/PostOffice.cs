namespace Ts.Domain.Models
{
    public class PostOffice : BaseEntity
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string Pincode { get; set; }
        public int DistrictId { get; set; }
        public virtual District District { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
