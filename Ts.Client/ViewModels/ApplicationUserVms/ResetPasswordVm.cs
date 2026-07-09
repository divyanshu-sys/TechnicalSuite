using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class ResetPasswordVm
    {
        [Display(Name = "New Password")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = ErrorMessageConstant.MinLength)]
        [RegularExpression(RegxConstant.Password, ErrorMessage = ErrorMessageConstant.PasswordRegx)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Compare("Password", ErrorMessage = ErrorMessageConstant.Compare)]
        public string ConfirmPassword { get; set; }
    }
}
