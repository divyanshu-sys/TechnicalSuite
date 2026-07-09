using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ClientUserVms
{
    public class UpdateClientUserVm
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string LastName { get; set; }
    }
}
