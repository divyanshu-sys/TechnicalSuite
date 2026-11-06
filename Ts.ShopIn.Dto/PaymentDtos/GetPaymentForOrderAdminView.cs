using Ts.ShopIn.Dto.PaymentStatusHistoryDtos;

namespace Ts.ShopIn.Dto.PaymentDtos
{
    public class GetPaymentForOrderAdminView : GetPaymentDataTableDto
    {
        public PaymentStatusHistoryDto PaymentStatusHistory { get; set; }
    }
}
