using Ts.Client.ViewModels;
using Ts.Client.ViewModels.PaymentGatewayTypeVms;
using Ts.Client.ViewModels.PaymentStatusVms;
using Ts.Common.HelperExtensions;

namespace Ts.ShopIn.Client.ViewModels.PaymentVms
{
    public class PaymentVm
    {
        public int Id { get; set; }
        public int? Pay_CurrencyTypeId { get; set; }
        public decimal? Pay_Amount { get; set; }
        public string Pay_Method { get; set; }
        public string Pay_Payment_Id { get; set; }
        public string Pay_Order_Id { get; set; }
        public decimal? Pay_Amount_Refunded { get; set; }
        public string Pay_Refund_Status { get; set; }
        public DateTime? Pay_Created_At { get; set; }
        public string Pay_Created_AtIst => Pay_Created_At?.ToDateTimeIstString();
        public int PaymentGatewayTypeId { get; set; }
        public int? PaymentStatusId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedOnIst => CreatedOn?.ToDateTimeIstString();
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedOnIst => UpdatedOn?.ToDateTimeIstString();

        public CurrencyTypeVm CurrencyType { get; set; }
        public PaymentGatewayTypeVm PaymentGatewayType { get; set; }
        public PaymentStatusVm PaymentStatus { get; set; }
    }
}
