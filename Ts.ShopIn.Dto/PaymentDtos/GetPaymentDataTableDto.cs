using Ts.Dto.PaymentGatewayTypeDtos;
using Ts.Dto.PaymentStatusDtos;

namespace Ts.ShopIn.Dto.PaymentDtos
{
    public class GetPaymentDataTableDto : PaymentDto
    {
        public PaymentGatewayTypeDto PaymentGatewayType { get; set; }
        public PaymentStatusDto PaymentStatus { get; set; }
    }
}
