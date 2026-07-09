using Ts.DotIn.Dto.PostImageDtos;
using Ts.DotIn.Dto.PostRelativeDtos;
namespace Ts.DotIn.Dto.PostDtos
{
    public class GetUpdatePostDto : PostDto
    {
        public PostImageDto PostImage { get; set; }
        public PostRelativeDto PostRelative { get; set; }
    }
}
