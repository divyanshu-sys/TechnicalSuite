namespace Ts.ShopIn.Domain.Models
{
    public class ProductVariant
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
