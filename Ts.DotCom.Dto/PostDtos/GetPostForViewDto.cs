using Ts.DotCom.Dto.PostRelativeDtos;
namespace Ts.DotCom.Dto.PostDtos
{
    public class GetPostForViewDto : PostDto
    {
        public PostRelativeDto PostRelative { get; set; }
    }
}
