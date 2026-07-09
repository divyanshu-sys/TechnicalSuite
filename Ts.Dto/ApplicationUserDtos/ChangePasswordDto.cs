using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.ApplicationUserDtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = ErrorMessageConstant.MinLength)]
        [RegularExpression(RegxConstant.Password, ErrorMessage = ErrorMessageConstant.PasswordRegx)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(16, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string IpAddress { get; set; }
    }
}
