using Ts.ShopIn.Dto.BlogImageDtos;

namespace Ts.ShopIn.Dto.BlogDtos
{
    public class GetUpdateBlogDto : BlogDto
    {
        public BlogImageDto BlogImage { get; set; }
    }
}
