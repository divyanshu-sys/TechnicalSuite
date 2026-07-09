namespace Ts.ShopIn.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public virtual Order Order { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal Amount { get; set; }
        public string Razorpay_Method { get; set; }
        public string Razorpay_Payment_Id { get; set; }
        public string Razorpay_Order_Id { get; set; }
        public decimal Razorpay_Amount_Refunded { get; set; }
        public string Razorpay_Refund_Status { get; set; }
        public long Razorpay_Created_At { get; set; }
        public int PaymentGatewayTypeId { get; set; }
        public int PaymentStatusId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual PaymentStatusHistory PaymentStatusHistory { get; set; }
    }
}
