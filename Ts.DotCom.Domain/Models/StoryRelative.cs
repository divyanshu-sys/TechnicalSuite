namespace Ts.DotCom.Domain.Models
{
    public class StoryRelative : BaseEntity
    {
        public new int Id { get; set; }
        public virtual Story Story { get; set; }
        public string RelativeUrl { get; set; }

    }
}
