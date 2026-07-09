using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.AddressVms
{
    public class UpdateAddressVm
    {
        [DisplayName("Address 1")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(450, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Address1 { get; set; }

        [DisplayName("Address 2")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(450, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Address2 { get; set; }

        [DisplayName("PostOffice")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int PostOfficeId { get; set; }

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
