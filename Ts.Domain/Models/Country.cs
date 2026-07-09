namespace Ts.Domain.Models
{
    public class Country : BaseEntity
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }

        public virtual ICollection<State> States { get; set; }
    }
}
