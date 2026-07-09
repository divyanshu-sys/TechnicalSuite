using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.SubCategoryDtos
{
    public class UpdateSubCategoryDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.LinkText, ErrorMessage = ErrorMessageConstant.LinkTextRegx)]
        public string Name { get; set; }
    }
}
