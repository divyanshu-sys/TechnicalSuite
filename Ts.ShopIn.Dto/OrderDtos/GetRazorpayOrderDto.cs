namespace Ts.ShopIn.Dto.OrderDtos
{
    public class GetRazorpayOrderDto
    {
        public string RazorpayApiKey { get; set; }
        public string Order_Id { get; set; }
        public string OrderNumber { get; set; }
        public string Description { get; set; }
        public bool IsPaymentRequired { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientFullPhoneCodeNumber { get; set; }
    }
}
