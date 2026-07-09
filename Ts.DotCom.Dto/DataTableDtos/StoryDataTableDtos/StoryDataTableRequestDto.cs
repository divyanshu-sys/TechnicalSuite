using Ts.Dto.DataTableDtos;
namespace Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos
{
    public class StoryDataTableRequestDto : BaseDataTableRequestDto<StoryOrderDto>
    {
        public string StoryWorkerId { get; set; }
    }
}
