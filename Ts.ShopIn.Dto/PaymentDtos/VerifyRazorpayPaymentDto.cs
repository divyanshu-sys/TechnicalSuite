using System.ComponentModel.DataAnnotations;

namespace Ts.ShopIn.Dto.PaymentDtos
{
    public class VerifyRazorpayPaymentDto
    {
        [Required]
        public string Razorpay_Payment_Id { get; set; }
        [Required]
        public string Razorpay_Order_Id { get; set; }
        [Required]
        public string Razorpay_Signature { get; set; }
    }
}
