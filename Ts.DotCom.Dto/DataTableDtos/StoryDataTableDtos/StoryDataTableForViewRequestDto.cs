using System.ComponentModel.DataAnnotations;
namespace Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos
{
    public class StoryDataTableForViewRequestDto
    {
        public string Search { get; set; }
        public string SubCategoryName { get; set; }

        [Range(0, int.MaxValue)]
        public int Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Length { get; set; }
    }
}
