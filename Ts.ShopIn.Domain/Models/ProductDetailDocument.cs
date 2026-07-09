namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetailDocument : BaseEntity
    {
        public new int Id { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public string Document { get; set; }
    }
}
