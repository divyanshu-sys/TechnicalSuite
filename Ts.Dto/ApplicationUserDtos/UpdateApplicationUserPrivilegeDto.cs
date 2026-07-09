using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.ApplicationUserDtos
{
    public class UpdateApplicationUserPrivilegeDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public List<string> RoleNames { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public List<string> ClaimTypes { get; set; }
    }
}
