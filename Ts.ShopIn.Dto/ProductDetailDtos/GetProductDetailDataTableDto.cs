using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DeliveryPolicyDtos;
using Ts.Dto.ExchangePolicyDtos;
using Ts.Dto.ReturnPolicyDtos;
using Ts.Dto.ShopCategoryDtos;
using Ts.ShopIn.Dto.ProductDetailViewDtos;
namespace Ts.ShopIn.Dto.ProductDetailDtos
{
    public class GetProductDetailDataTableDto : ProductDetailDto
    {
        public ProductDetailViewDto ProductDetailView { get; set; }
        public ShopCategoryDto ShopCategory { get; set; }
        public ExchangePolicyDto ExchangePolicy { get; set; }
        public DeliveryPolicyDto DeliveryPolicy { get; set; }
        public ReturnPolicyDto ReturnPolicy { get; set; }
        public ApplicationUserDto PublishedBy { get; set; }
        public ApplicationUserDto ProductDetailWorker { get; set; }
    }
}
