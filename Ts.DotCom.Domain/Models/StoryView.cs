namespace Ts.DotCom.Domain.Models
{
    public class StoryView
    {
        public int Id { get; set; }
        public virtual Story Story { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }

    }
}
