namespace Ts.Domain.Models
{
    public class District : BaseEntity
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }

        public virtual ICollection<PostOffice> PostOffices { get; set; }
    }
}
