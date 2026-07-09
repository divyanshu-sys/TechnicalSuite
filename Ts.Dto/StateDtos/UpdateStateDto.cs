using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.StateDtos
{
    public class UpdateStateDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(2, MinimumLength = 2, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Code2 { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int CountryId { get; set; }
    }
}
