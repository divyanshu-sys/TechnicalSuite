namespace Ts.Common.Constant.SiteConstants
{
    public static class OrderStatusConstant
    {
        public const string OrderInitiated = "Order-Initiated";
        public const string OrderCompleted = "Order-Completed";
        public const string OrderPaymentPending = "Payment-Pending";
        public const string OrderPaymentRefunded = "Payment-Refunded";
        public const string OrderPaymentFailed = "Payment-Failed";
        public const string OrderPaymentRefundInProcess = "Payment-Refund-In-Process";
        public const string OrderPaymentRefundFailed = "Payment-Refund-Failed";

        public static string GetOrderStatusMessageBy(string orderStatusConstant)
        {
            return (orderStatusConstant ?? string.Empty) switch
            {
                OrderInitiated => "Order initiated.",
                OrderCompleted => "Order completed successfully.",
                OrderPaymentPending => "Order payment is pending.",
                OrderPaymentRefunded => "Order payment refunded successfully.",
                OrderPaymentFailed => "Order payment failed.",
                OrderPaymentRefundInProcess => "Order payment refund is in process.",
                OrderPaymentRefundFailed => "Order payment refund failed.",
                _ => null
            };
        }
    }
}
