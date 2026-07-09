namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetailView
    {
        public int Id { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }

    }
}
