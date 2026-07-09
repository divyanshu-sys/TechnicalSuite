using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductDetailDtos
{
    public class UpdateProductDetailMainImageDto : BaseInputDto
    {
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImage { get; set; }
    }
}
