namespace Ts.Domain.Models
{
    public class State : BaseEntity
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string Code2 { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
