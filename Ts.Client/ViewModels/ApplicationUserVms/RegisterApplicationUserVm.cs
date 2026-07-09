using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Ts.Client.ViewModels.AddressVms;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class RegisterApplicationUserVm
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string LastName { get; set; }

        [PageRemote(PageHandler = "IsEmailAvailable")]
        [Display(Name = "Email ID")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(256, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Email, ErrorMessage = ErrorMessageConstant.EmailRegx)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = ErrorMessageConstant.MinLength)]
        [RegularExpression(RegxConstant.Password, ErrorMessage = ErrorMessageConstant.PasswordRegx)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Compare(nameof(Password), ErrorMessage = ErrorMessageConstant.Compare)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public CreateAddressVm Address { get; set; }
    }
}
