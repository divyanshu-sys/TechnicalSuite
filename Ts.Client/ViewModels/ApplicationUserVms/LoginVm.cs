using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class LoginVm
    {
        [DisplayName("Login ID")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me on this device")]
        public bool RememberMe { get; set; }
    }
}
