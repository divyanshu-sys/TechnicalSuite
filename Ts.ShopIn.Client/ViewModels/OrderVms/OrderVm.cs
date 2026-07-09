using Ts.Client.ViewModels;
using Ts.Common.HelperExtensions;
using Ts.ShopIn.Client.ViewModels.OrderDetailVms;

namespace Ts.ShopIn.Client.ViewModels.OrderVms
{
    public class OrderVm
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ClientName { get; set; }
        public int PaymentModeId { get; set; }
        public int TotalItemCount { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public DateTime OrderedOn { get; set; }
        public string OrderedOnIst => OrderedOn.ToDateTimeIstString();
        public decimal RefundedAmount { get; set; }

        public CurrencyTypeVm CurrencyType { get; set; }
        public List<OrderDetailVm> OrderDetailsForView { get; set; }
    }
}
