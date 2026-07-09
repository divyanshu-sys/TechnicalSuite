using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class UpdatePrivilegeVm : ApplicationUserVm
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public List<UpdateUserRoleVm> UserRoles { get; set; }


        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public List<UpdateUserClaimVm> UserClaims { get; set; }
    }

    public class UpdateUserRoleVm
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UpdateUserClaimVm
    {
        public UpdateUserClaimVm()
        {
            Policies = new();
        }

        public string RoleName { get; set; }
        public List<UserPolicyVm> Policies { get; set; }
    }

    public class UserClaimVm
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UserPolicyVm
    {
        public UserPolicyVm()
        {
            Claims = new();
        }

        public string PolicyName { get; set; }
        public List<UserClaimVm> Claims { get; set; }
    }
}
