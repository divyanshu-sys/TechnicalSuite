using Ts.DotIn.Dto.StoryImageDtos;
using Ts.DotIn.Dto.StoryRelativeDtos;
namespace Ts.DotIn.Dto.StoryDtos
{
    public class GetUpdateStoryDto : StoryDto
    {
        public StoryImageDto StoryImage { get; set; }
        public StoryRelativeDto StoryRelative { get; set; }
    }
}
