using Ts.Dto;
namespace Ts.DotIn.Dto.StoryImageDtos
{
    public class StoryImageDto : BaseDto
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public string ImageBaseUrl { get; set; }
    }
}
