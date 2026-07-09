namespace Ts.Dto.ApplicationUserDtos
{
    public class GetApplicationUserPrivilegeDto : ApplicationUserDto
    {
        public GetApplicationUserPrivilegeDto()
        {
            UserRoles = new();
            UserClaims = new();
        }
        public List<GetUserRoleDto> UserRoles { get; set; }
        public List<GetUserClaimDto> UserClaims { get; set; }
    }

    public class GetUserRoleDto
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

    public class GetUserClaimDto
    {
        public GetUserClaimDto()
        {
            Policies = new();
        }

        public string RoleName { get; set; }
        public List<GetUserPolicyDto> Policies { get; set; }
    }

    public class UserClaimDto
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }

    public class GetUserPolicyDto
    {
        public GetUserPolicyDto()
        {
            Claims = new();
        }

        public string PolicyName { get; set; }
        public List<UserClaimDto> Claims { get; set; }
    }
}
