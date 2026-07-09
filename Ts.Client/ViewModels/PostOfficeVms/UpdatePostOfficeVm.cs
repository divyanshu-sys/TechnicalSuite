using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.PostOfficeVms
{
    public class UpdatePostOfficeVm
    {
        [DisplayName("Post Office Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [DisplayName("Pincode")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Pincode { get; set; }

        [DisplayName("District")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int DistrictId { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int StateId { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int CountryId { get; set; }
    }
}
