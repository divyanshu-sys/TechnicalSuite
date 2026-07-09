using System.ComponentModel.DataAnnotations;
using Ts.Client.ViewModels.AddressVms;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class UpdateApplicationUserVm
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public UpdateAddressVm Address { get; set; }
    }
}
