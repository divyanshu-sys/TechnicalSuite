using Ts.Dto.DataTableDtos;
namespace Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos
{
    public class PostDataTableRequestDto : BaseDataTableRequestDto<PostOrderDto>
    {
        public string PostWorkerId { get; set; }
        public DateTimeOffset? LastViewedOnStart { get; set; }
        public DateTimeOffset? LastViewedOnEnd { get; set; }
    }
}
