using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.CountryVms
{
    public class CreateCountryVm
    {
        [DisplayName("Country Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [DisplayName("Country Code2")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(2, MinimumLength = 2, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Code2 { get; set; }

        [DisplayName("Country Code3")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(3, MinimumLength = 3, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Code3 { get; set; }
    }
}
