using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class ChangePasswordVm
    {
        [DisplayName("Current Password")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DisplayName("New Password")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = ErrorMessageConstant.MinLength)]
        [RegularExpression(RegxConstant.Password, ErrorMessage = ErrorMessageConstant.PasswordRegx)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Compare(nameof(Password), ErrorMessage = ErrorMessageConstant.Compare)]
        public string ConfirmPassword { get; set; }
    }
}
