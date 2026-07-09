using Ts.Dto;
namespace Ts.DotIn.Dto.PostImageDtos
{
    public class PostImageDto : BaseDto
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public string ImageBaseUrl { get; set; }
    }
}
