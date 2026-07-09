using Ts.DotCom.Dto.StoryRelativeDtos;
namespace Ts.DotCom.Dto.StoryDtos
{
    public class GetStoryForViewDto : StoryDto
    {
        public StoryRelativeDto StoryRelative { get; set; }
    }
}
