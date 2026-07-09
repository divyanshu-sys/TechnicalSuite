using Ts.Dto.DataTableDtos;
namespace Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos
{
    public class BlogDataTableRequestDto : BaseDataTableRequestDto<BlogOrderDto>
    {
        public string BlogWorkerId { get; set; }
        public DateTimeOffset? LastViewedOnStart { get; set; }
        public DateTimeOffset? LastViewedOnEnd { get; set; }
    }
}
