namespace Ts.DotCom.Domain.Models
{
    public class StoryImage : BaseEntity
    {
        public new int Id { get; set; }
        public virtual Story Story { get; set; }
        public string ImageNames { get; set; }

    }
}
