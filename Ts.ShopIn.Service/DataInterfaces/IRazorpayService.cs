using Ts.ShopIn.Service.HelperModel;

namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IRazorpayService
    {
        Task<RazorPayOrderHelperModel> CreateOrderAsync(decimal totalAmount, string currencyLetter, string orderNumber, string apiKey, string keySecret);
        Task<RazorpayPaymentHelperModel> VerifyOrderPaymentAsync(string payment_Id, string order_Id, string payment_Signature, string apiKey, string keySecret);
        Task<RazorpayPaymentHelperModel> GetPaymentByPaymentIdAsync(string payment_Id, string apiKey, string keySecret);
        Task<RazorpayPaymentHelperModel> GetPaymentByOrderIdAsync(string order_Id, string apiKey, string keySecret);
        Task<RazorpayPaymentHelperModel> VerifyWebhookAsync(string requestBody, string webhook_Signature, string webhookSecret);
    }
}
