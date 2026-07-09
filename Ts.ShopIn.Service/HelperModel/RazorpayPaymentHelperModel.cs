namespace Ts.ShopIn.Service.HelperModel
{
    public class RazorpayPaymentHelperModel
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public long CreatedAt { get; set; }
        public string Method { get; set; }
        public long AmountRefunded { get; set; }
        public string RefundStatus { get; set; }
        public string Currency { get; set; }
    }
}
