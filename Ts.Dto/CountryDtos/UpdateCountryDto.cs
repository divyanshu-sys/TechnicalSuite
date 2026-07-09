using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.CountryDtos
{
    public class UpdateCountryDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(2, MinimumLength = 2, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Code2 { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(3, MinimumLength = 3, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Code3 { get; set; }
    }
}
