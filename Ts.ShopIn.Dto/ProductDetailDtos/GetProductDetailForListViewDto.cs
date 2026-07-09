namespace Ts.ShopIn.Dto.ProductDetailDtos
{
    public class GetProductDetailForListViewDto : CurrencyBaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ShopCategoryId { get; set; }
        public string ShopCategoryName { get; set; }
        public string ProductDetailLink { get; set; }
        public string MainImageUrl { get; set; }
        public decimal Mrp { get; set; }
        public decimal Price { get; set; }
        public DateTime? PublishedOn { get; set; }
        public bool IsAvailable { get; set; }
        public int ExchangePolicyId { get; set; }
        public int DeliveryPolicyId { get; set; }
        public int ReturnPolicyId { get; set; }
    }
}
