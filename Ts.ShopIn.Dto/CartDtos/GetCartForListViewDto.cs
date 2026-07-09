namespace Ts.ShopIn.Dto.CartDtos
{
    public class GetCartForListViewDto : CurrencyBaseDto
    {
        public GetCartForListViewDto()
        {
            ProductDetails = new();
        }
        public decimal TotalAvailableItemAmount { get; set; }
        public int TotalAvailableItemCount { get; set; }
        public List<GetCartForViewDto> ProductDetails { get; set; }
    }
}
