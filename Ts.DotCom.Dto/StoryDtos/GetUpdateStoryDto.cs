using Ts.DotCom.Dto.StoryImageDtos;
using Ts.DotCom.Dto.StoryRelativeDtos;
namespace Ts.DotCom.Dto.StoryDtos
{
    public class GetUpdateStoryDto : StoryDto
    {
        public StoryImageDto StoryImage { get; set; }
        public StoryRelativeDto StoryRelative { get; set; }
    }
}
