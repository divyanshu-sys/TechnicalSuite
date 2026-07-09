namespace Ts.Common.Constant.SiteConstants
{
    public static class PaymentGatewayTypeConstant
    {
        public const string Razorpay = "Razorpay";
    }

    public static class RazorpayOrderStatusConstant
    {
        public const string Created = "created";
        public const string Attempted = "attempted";
        public const string Paid = "paid";
    }

    public static class RazorpayPaymentStatusConstant
    {
        public const string Created = "created";
        public const string Authorized = "authorized";
        public const string Captured = "captured";
        public const string Refunded = "refunded";
        public const string Failed = "failed";

        private static readonly Dictionary<string, List<string>> validTransitions = new()
        {
            { Created, new List<string> { Authorized, Failed, Captured, Refunded } },
            { Authorized, new List<string> { Captured, Failed, Refunded } },
            { Captured, new List<string> { Refunded } },
            { Failed, new List<string>() },
            { Refunded, new List<string>() }
        };

        public static bool IsValidTransition(string oldRazorpayPaymentStatus, string newRazorpayPaymentStatus)
        {
            bool isValidTransition = string.IsNullOrEmpty(oldRazorpayPaymentStatus)
                || (validTransitions.ContainsKey(oldRazorpayPaymentStatus)
                && validTransitions[oldRazorpayPaymentStatus].Contains(newRazorpayPaymentStatus));
            return isValidTransition;
        }

        public static string GetOrderStatusConstantBy(string razorpayPaymentStatus)
        {
            return (razorpayPaymentStatus ?? string.Empty).ToLowerInvariant() switch
            {
                Created => OrderStatusConstant.OrderInitiated,
                Authorized => OrderStatusConstant.OrderPaymentPending,
                Captured => OrderStatusConstant.OrderCompleted,
                Refunded => OrderStatusConstant.OrderPaymentRefunded,
                Failed => OrderStatusConstant.OrderPaymentFailed,
                _ => null
            };
        }

        public static string GetPaymentStatusConstantStatusMessageBy(string razorpayPaymentStatus)
        {
            return (razorpayPaymentStatus ?? string.Empty).ToLowerInvariant() switch
            {
                Created => "Payment initiated successfully.",
                Captured => "Payment completed successfully.",
                Authorized => "Payment is pending.",
                Refunded => "Payment refunded successfully. Amount will be credited to your account within 5-7 working days.",
                Failed => "Payment failed.",
                _ => null
            };
        }
    }
}
