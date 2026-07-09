using Ts.Dto.CurrencyTypeDtos;
using Ts.ShopIn.Dto.OrderDetailDtos;

namespace Ts.ShopIn.Dto.OrderDtos
{
    public class GetOrderForViewDto : OrderDto
    {
        public CurrencyTypeDto CurrencyType { get; set; }
        public List<GetOrderDetailForViewDto> OrderDetailsForView { get; set; }
        public decimal RefundedAmount { get; set; }
    }
}
