using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Razorpay.Api;
using Ts.ShopIn.Service.DataInterfaces;
using Ts.ShopIn.Service.HelperModel;

namespace Ts.ShopIn.Service.DataServices
{
    public class RazorpayService : IRazorpayService
    {
        private readonly ILogger<RazorpayService> logger;

        public RazorpayService(ILogger<RazorpayService> logger)
        {
            this.logger = logger;
        }

        public Task<RazorPayOrderHelperModel> CreateOrderAsync(decimal totalAmount, string currencyLetter, string orderNumber, string apiKey, string keySecret)
        {
            long totalAmountSubunits = CommonProductDetailService.RoundingToInteger(totalAmount, currencyLetter);

            var input = new Dictionary<string, object>
            {
                { "amount", totalAmountSubunits },
                { "currency", currencyLetter },
                { "receipt", orderNumber },
                { "notes", new Dictionary<string, string>
                           {
                               { "orderNumber", orderNumber }
                           }
                }
            };

            try
            {
                var client = new RazorpayClient(apiKey, keySecret);
                var order = client.Order.Create(input);

                if (order != null && order["id"].Value != null)
                    return Task.FromResult(new RazorPayOrderHelperModel()
                    {
                        Id = order["id"].Value
                    });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating razorpay order.");
            }

            return Task.FromResult<RazorPayOrderHelperModel>(null);
        }

        public Task<RazorpayPaymentHelperModel> GetPaymentByPaymentIdAsync(string payment_Id, string apiKey, string keySecret)
        {
            try
            {
                var client = new RazorpayClient(apiKey, keySecret);

                var payment = client.Payment.Fetch(payment_Id);

                if (payment != null && payment["id"].Value != null)
                    return Task.FromResult(MapPaymentToModel(payment));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching razorpay payment.");
            }

            return Task.FromResult<RazorpayPaymentHelperModel>(null);
        }

        public Task<RazorpayPaymentHelperModel> GetPaymentByOrderIdAsync(string order_Id, string apiKey, string keySecret)
        {
            try
            {
                var client = new RazorpayClient(apiKey, keySecret);

                var order = client.Order.Fetch(order_Id);
                var payments = order.Payments();
                var payment = payments.OrderByDescending(x => (long)x["created_at"]).FirstOrDefault();

                if (payment != null && payment["id"].Value != null)
                    return Task.FromResult(MapPaymentToModel(payment));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching razorpay payment.");
            }

            return Task.FromResult<RazorpayPaymentHelperModel>(null);
        }

        public Task<RazorpayPaymentHelperModel> VerifyOrderPaymentAsync(string payment_Id, string order_Id, string payment_Signature, string apiKey, string keySecret)
        {
            var attributes = new Dictionary<string, string>
            {
                { "razorpay_payment_id", payment_Id },
                { "razorpay_order_id", order_Id },
                { "razorpay_signature", payment_Signature }
            };

            try
            {
                Utils.verifyPaymentSignature(attributes);

                var client = new RazorpayClient(apiKey, keySecret);
                var payment = client.Payment.Fetch(payment_Id);

                if (payment != null && payment["id"].Value != null)
                    return Task.FromResult(MapPaymentToModel(payment));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while verifying razorpay order payment.");
            }

            return Task.FromResult<RazorpayPaymentHelperModel>(null);
        }

        public Task<RazorpayPaymentHelperModel> VerifyWebhookAsync(string requestBody, string webhook_Signature, string webhookSecret)
        {
            try
            {
                Utils.verifyWebhookSignature(requestBody, webhook_Signature, webhookSecret);

                var data = JObject.Parse(requestBody);

                var payment = data["payload"]?["payment"]?["entity"];

                if (payment != null && payment["id"] != null)
                    return Task.FromResult(MapPaymentToModel(payment));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while verifying razorpay webhook.");
            }
            return Task.FromResult<RazorpayPaymentHelperModel>(null);
        }

        private static RazorpayPaymentHelperModel MapPaymentToModel(dynamic payment)
        {
            return new RazorpayPaymentHelperModel()
            {
                Id = payment["id"].Value,
                OrderId = payment["order_id"].Value,
                Status = payment["status"].Value,
                CreatedAt = payment["created_at"].Value,
                Method = payment["method"].Value,
                AmountRefunded = payment["amount_refunded"].Value,
                RefundStatus = payment["refund_status"].Value,
                Currency = payment["currency"].Value
            };
        }
    }
}
