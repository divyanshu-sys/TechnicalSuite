using Ts.Dto;
namespace Ts.ShopIn.Dto.BlogImageDtos
{
    public class BlogImageDto : BaseDto
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public string ImageBaseUrl { get; set; }
    }
}
