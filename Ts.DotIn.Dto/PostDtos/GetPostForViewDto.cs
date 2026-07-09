using Ts.DotIn.Dto.PostRelativeDtos;
namespace Ts.DotIn.Dto.PostDtos
{
    public class GetPostForViewDto : PostDto
    {
        public PostRelativeDto PostRelative { get; set; }
    }
}
