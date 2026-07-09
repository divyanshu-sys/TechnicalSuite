using Ts.DotCom.Dto.PostViewDtos;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.CategoryDtos;
using Ts.Dto.SubCategoryDtos;
namespace Ts.DotCom.Dto.PostDtos
{
    public class GetPostDataTableDto : PostDto
    {
        public PostViewDto PostView { get; set; }
        public CategoryDto Category { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public ApplicationUserDto PublishedBy { get; set; }
        public ApplicationUserDto PostWorker { get; set; }
    }
}
