using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductDtos
{
    public class UpdateProductDto : BaseInputDto
    {
        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(120, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public bool IsAvailable { get; set; }
    }
}
