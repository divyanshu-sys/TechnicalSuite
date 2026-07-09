using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto.AddressDtos;
namespace Ts.Dto.ApplicationUserDtos
{
    public class UpdateApplicationUserDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string LastName { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public UpdateAddressDto Address { get; set; }
    }
}
