using System.ComponentModel.DataAnnotations;
namespace Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos
{
    public class ProductDetailDataTableForViewRequestDto
    {
        public string Search { get; set; }
        public string ShopCategoryName { get; set; }

        [Range(0, int.MaxValue)]
        public int Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Length { get; set; }
    }
}
