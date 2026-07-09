namespace Ts.ShopIn.Domain.Models
{
    public class BlogImage : BaseEntity
    {
        public new int Id { get; set; }
        public virtual Blog Blog { get; set; }
        public string ImageNames { get; set; }
    }
}
