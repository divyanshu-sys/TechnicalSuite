using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.StateVms
{
    public class UpdateStateVm
    {
        [DisplayName("State Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [DisplayName("State Code2")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(2, MinimumLength = 2, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Code2 { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int CountryId { get; set; }
    }
}
