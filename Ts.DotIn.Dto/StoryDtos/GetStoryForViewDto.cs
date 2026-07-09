using Ts.DotIn.Dto.StoryRelativeDtos;
namespace Ts.DotIn.Dto.StoryDtos
{
    public class GetStoryForViewDto : StoryDto
    {
        public StoryRelativeDto StoryRelative { get; set; }
    }
}
