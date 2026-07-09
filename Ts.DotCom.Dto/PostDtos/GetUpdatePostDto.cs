using Ts.DotCom.Dto.PostImageDtos;
using Ts.DotCom.Dto.PostRelativeDtos;
namespace Ts.DotCom.Dto.PostDtos
{
    public class GetUpdatePostDto : PostDto
    {
        public PostImageDto PostImage { get; set; }
        public PostRelativeDto PostRelative { get; set; }
    }
}
