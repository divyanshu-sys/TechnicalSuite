namespace Ts.ShopIn.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public virtual Order Order { get; set; }
        public int? Pay_CurrencyTypeId { get; set; }
        public decimal? Pay_Amount { get; set; }
        public string Pay_Method { get; set; }
        public string Pay_Payment_Id { get; set; }
        public string Pay_Order_Id { get; set; }
        public decimal? Pay_Amount_Refunded { get; set; }
        public string Pay_Refund_Status { get; set; }
        public DateTime? Pay_Created_At { get; set; }
        public int PaymentGatewayTypeId { get; set; }
        public int? PaymentStatusId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual PaymentStatusHistory PaymentStatusHistory { get; set; }
    }
}
