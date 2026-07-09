using System.ComponentModel.DataAnnotations;
namespace Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos
{
    public class ProductDataTableForViewRequestDto
    {
        public string Search { get; set; }

        [Range(0, int.MaxValue)]
        public int Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Length { get; set; }
    }
}
