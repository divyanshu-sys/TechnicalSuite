using Ts.Dto.DeliveryPolicyDtos;
using Ts.Dto.ExchangePolicyDtos;
using Ts.Dto.ReturnPolicyDtos;
using Ts.Dto.ShopCategoryDtos;

namespace Ts.ShopIn.Dto.ProductDetailDtos
{
    public class GetForViewProductDetailDto : CurrencyBaseDto
    {
        public GetForViewProductDetailDto()
        {
            ProductVariants = new();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProductDetailLink { get; set; }
        public string MainImageUrl { get; set; }
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
        public decimal Mrp { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedById { get; set; }

        public ExchangePolicyDto ExchangePolicy { get; set; }
        public DeliveryPolicyDto DeliveryPolicy { get; set; }
        public ReturnPolicyDto ReturnPolicy { get; set; }
        public ShopCategoryDto ShopCategory { get; set; }

        public List<GetProductDetailForListViewDto> ProductVariants { get; set; }
    }
}
