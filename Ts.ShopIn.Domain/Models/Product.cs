namespace Ts.ShopIn.Domain.Models
{
    public class Product : BaseEntity
    {
        public new int Id { get; set; }
        public string Title { get; set; }
        public string MainImage { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedById { get; set; }
        public string ProductWorkerId { get; set; }

        public virtual ProductImage ProductImage { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
