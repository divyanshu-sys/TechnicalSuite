using Ts.Dto.CurrencyTypeDtos;
using Ts.Dto.DeliveryPolicyDtos;
using Ts.Dto.OrderStatusDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;

namespace Ts.ShopIn.Dto.OrderDetailDtos
{
    public class GetOrderDetailForViewDto : OrderDetailDto
    {
        public OrderStatusDto OrderStatus { get; set; }
        public CurrencyTypeDto CurrencyType { get; set; }
        public DeliveryPolicyDto DeliveryPolicy { get; set; }
        public GetProductDetailForListViewDto ProductDetailForView { get; set; }
        public bool IsDownloadable { get; set; }
        public int IsDownloadableRemainingDays { get; set; }
    }
}
