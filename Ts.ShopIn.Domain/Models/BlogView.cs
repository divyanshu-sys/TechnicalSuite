namespace Ts.ShopIn.Domain.Models
{
    public class BlogView
    {
        public int Id { get; set; }
        public virtual Blog Blog { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }
    }
}
