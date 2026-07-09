using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.RefreshTokenDtos
{
    public class AuthRefreshDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string EncodedRefreshToken { get; set; }
    }
}
