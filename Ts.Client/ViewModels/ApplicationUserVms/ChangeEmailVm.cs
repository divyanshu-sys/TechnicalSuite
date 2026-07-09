using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class ChangeEmailVm
    {
        [PageRemote(PageHandler = "IsEmailAvailable")]
        [Display(Name = "New Email")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(256, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Email, ErrorMessage = ErrorMessageConstant.EmailRegx)]
        public string Email { get; set; }

        [DisplayName("Confirm Email")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Compare(nameof(Email), ErrorMessage = ErrorMessageConstant.Compare)]
        public string ConfirmEmail { get; set; }
    }
}
