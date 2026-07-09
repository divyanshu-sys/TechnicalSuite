namespace Ts.ShopIn.Domain.Models
{
    public class Cart : ClientBaseEntity
    {
        public int ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public int ItemCount { get; set; }
        public string UserId { get; set; }
        public virtual ClientUser CartAddedBy { get; set; }
    }
}
