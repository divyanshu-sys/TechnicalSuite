using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductDetailDtos
{
    public class UpdateProductDetailDescriptionDto : BaseInputDto
    {
        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Description { get; set; }
    }
}
