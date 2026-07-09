namespace Ts.DotCom.Domain.Models
{
    public class PostView
    {
        public int Id { get; set; }
        public virtual Post Post { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }

    }
}
