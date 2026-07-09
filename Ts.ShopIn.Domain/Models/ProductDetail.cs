namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetail : BaseEntity
    {
        public new int Id { get; set; }
        public string Title { get; set; }
        public string ProductDetailLink { get; set; }
        public string MainImage { get; set; }
        public string MetaDescription { get; set; }
        public int ShopCategoryId { get; set; }
        public int ExchangePolicyId { get; set; }
        public int DeliveryPolicyId { get; set; }
        public int ReturnPolicyId { get; set; }
        public string Description { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string Keyword4 { get; set; }
        public string Keyword5 { get; set; }
        public bool IsAvailable { get; set; }
        public int Stock { get; set; }
        public decimal Mrp { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedById { get; set; }
        public string ProductDetailWorkerId { get; set; }

        public virtual ProductDetailImage ProductDetailImage { get; set; }
        public virtual ProductDetailView ProductDetailView { get; set; }
        public virtual ProductDetailDocument ProductDetailDocument { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
