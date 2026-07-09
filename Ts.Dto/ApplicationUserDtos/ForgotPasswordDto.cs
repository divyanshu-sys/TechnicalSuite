using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.ApplicationUserDtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Email { get; set; }
    }
}
