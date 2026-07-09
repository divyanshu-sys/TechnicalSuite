namespace Ts.ShopIn.Domain.Models
{
    public class ProductImage : BaseEntity
    {
        public new int Id { get; set; }
        public virtual Product Product { get; set; }
        public string ImageNames { get; set; }
    }
}
