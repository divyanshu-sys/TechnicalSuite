using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.ApplicationUserDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string IpAddress { get; set; }
    }
}
