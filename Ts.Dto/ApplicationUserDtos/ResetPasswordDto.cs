using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.ApplicationUserDtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = ErrorMessageConstant.MinLength)]
        [RegularExpression(RegxConstant.Password, ErrorMessage = ErrorMessageConstant.PasswordRegx)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string EncUserId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Token { get; set; }
    }
}
