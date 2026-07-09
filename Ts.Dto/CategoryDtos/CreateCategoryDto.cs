using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.CategoryDtos
{
    public class CreateCategoryDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.LinkText, ErrorMessage = ErrorMessageConstant.LinkTextRegx)]
        public string Name { get; set; }
    }
}
