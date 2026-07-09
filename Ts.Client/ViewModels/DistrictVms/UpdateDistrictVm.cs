using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.DistrictVms
{
    public class UpdateDistrictVm
    {
        [DisplayName("District Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int CountryId { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int StateId { get; set; }
    }
}
