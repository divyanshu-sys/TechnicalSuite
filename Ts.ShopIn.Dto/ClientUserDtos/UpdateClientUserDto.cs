using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.ShopIn.Dto.ClientUserDtos
{
    public class UpdateClientUserDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string LastName { get; set; }
    }
}
