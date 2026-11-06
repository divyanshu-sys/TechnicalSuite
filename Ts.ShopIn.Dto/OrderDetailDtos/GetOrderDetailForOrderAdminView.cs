using Ts.Dto.CurrencyTypeDtos;
using Ts.Dto.OrderStatusDtos;
using Ts.ShopIn.Dto.OrderDetailStatusHistoryDtos;

namespace Ts.ShopIn.Dto.OrderDetailDtos
{
    public class GetOrderDetailForOrderAdminView : OrderDetailDto
    {
        public OrderStatusDto OrderStatus { get; set; }
        public CurrencyTypeDto CurrencyType { get; set; }
        public OrderDetailStatusHistoryDto OrderDetailStatusHistory { get; set; }
    }
}
