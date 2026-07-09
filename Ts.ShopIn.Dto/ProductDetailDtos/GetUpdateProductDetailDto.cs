using Ts.Dto.DeliveryPolicyDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Dto.ProductDetailImageDtos;
using Ts.ShopIn.Dto.ProductVariantDtos;
namespace Ts.ShopIn.Dto.ProductDetailDtos
{
    public class GetUpdateProductDetailDto : ProductDetailDto
    {
        public ProductVariantDto ProductVariant { get; set; }
        public ProductDetailImageDto ProductDetailImage { get; set; }
        public ProductDetailDocumentDto ProductDetailDocument { get; set; }
        public DeliveryPolicyDto DeliveryPolicy { get; set; }
    }
}
