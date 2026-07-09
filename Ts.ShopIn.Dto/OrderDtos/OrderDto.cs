namespace Ts.ShopIn.Dto.OrderDtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ClientName { get; set; }
        public int PaymentModeId { get; set; }
        public int TotalItemCount { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public DateTime OrderedOn { get; set; }
    }
}
