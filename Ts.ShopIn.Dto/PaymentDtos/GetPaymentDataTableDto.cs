using Ts.Dto.CurrencyTypeDtos;
using Ts.Dto.PaymentGatewayTypeDtos;
using Ts.Dto.PaymentStatusDtos;

namespace Ts.ShopIn.Dto.PaymentDtos
{
    public class GetPaymentDataTableDto : PaymentDto
    {
        public CurrencyTypeDto CurrencyType { get; set; }
        public PaymentGatewayTypeDto PaymentGatewayType { get; set; }
        public PaymentStatusDto PaymentStatus { get; set; }
    }
}
