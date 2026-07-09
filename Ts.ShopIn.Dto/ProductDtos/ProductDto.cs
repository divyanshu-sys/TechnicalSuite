using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductDtos
{
    public class ProductDto : BaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedById { get; set; }
        public string ProductWorkerId { get; set; }
    }
}
