namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetailImage : BaseEntity
    {
        public new int Id { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public string ImageNames { get; set; }
    }
}
