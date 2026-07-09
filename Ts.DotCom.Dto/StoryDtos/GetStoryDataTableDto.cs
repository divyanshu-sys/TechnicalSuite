using Ts.DotCom.Dto.StoryViewDtos;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.SubCategoryDtos;
namespace Ts.DotCom.Dto.StoryDtos
{
    public class GetStoryDataTableDto : StoryDto
    {
        public StoryViewDto StoryView { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public ApplicationUserDto PublishedBy { get; set; }
        public ApplicationUserDto StoryWorker { get; set; }
    }
}
