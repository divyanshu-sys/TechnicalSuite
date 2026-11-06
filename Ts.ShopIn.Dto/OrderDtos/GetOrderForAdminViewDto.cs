using Ts.Dto.CurrencyTypeDtos;
using Ts.Dto.PaymentModeDtos;
using Ts.ShopIn.Dto.ClientUserDtos;
using Ts.ShopIn.Dto.OrderDetailDtos;
using Ts.ShopIn.Dto.PaymentDtos;

namespace Ts.ShopIn.Dto.OrderDtos
{
    public class GetOrderForAdminViewDto : OrderDto
    {
        public CurrencyTypeDto CurrencyType { get; set; }
        public GetClientUserForOrderAdminView OrderedBy { get; set; }
        public GetPaymentForOrderAdminView Payment { get; set; }
        public PaymentModeDto PaymentMode { get; set; }
        public virtual List<GetOrderDetailForOrderAdminView> OrderDetails { get; set; }
    }
}
