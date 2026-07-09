using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.ApplicationUserDtos
{
    public class ConfirmChangeEmailDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string EncUserId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string EncEmail { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Token { get; set; }
    }
}
