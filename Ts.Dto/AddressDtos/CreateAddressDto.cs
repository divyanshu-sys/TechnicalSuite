using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.AddressDtos
{
    public class CreateAddressDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(450, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Address1 { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(450, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Address2 { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int PostOfficeId { get; set; }
    }
}
