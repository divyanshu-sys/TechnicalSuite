namespace Ts.DotIn.Domain.Models
{
    public class PostImage : BaseEntity
    {
        public new int Id { get; set; }
        public virtual Post Post { get; set; }
        public string ImageNames { get; set; }

    }
}
