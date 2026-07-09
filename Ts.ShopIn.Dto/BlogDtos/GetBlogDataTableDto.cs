using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.SubCategoryDtos;
using Ts.ShopIn.Dto.BlogViewDtos;
namespace Ts.ShopIn.Dto.BlogDtos
{
    public class GetBlogDataTableDto : BlogDto
    {
        public BlogViewDto BlogView { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public ApplicationUserDto PublishedBy { get; set; }
        public ApplicationUserDto BlogWorker { get; set; }
    }
}
