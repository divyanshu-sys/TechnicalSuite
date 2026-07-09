using System.ComponentModel.DataAnnotations;

namespace Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos
{
    public class OrderDataTableForViewRequestDto
    {
        [Range(0, int.MaxValue)]
        public int Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Length { get; set; }
    }
}
