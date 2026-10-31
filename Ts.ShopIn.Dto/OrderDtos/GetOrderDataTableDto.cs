using Ts.Dto.CurrencyTypeDtos;
using Ts.ShopIn.Dto.PaymentDtos;

namespace Ts.ShopIn.Dto.OrderDtos
{
    public class GetOrderDataTableDto : OrderDto
    {
        public CurrencyTypeDto CurrencyType { get; set; }
        public GetPaymentDataTableDto Payment { get; set; }
    }
}
