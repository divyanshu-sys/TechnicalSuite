using Ts.Common.Constant.AppConstants.PolicyConstants;
namespace Ts.Common.Constant.AppConstants
{
    public static class RoleConstant
    {
        public const string Administrator = "Administrator";
        public const string Employee = "Employee";
        public const string DotInSiteUser = "DotInSiteUser";
        public const string DotComSiteUser = "DotComSiteUser";
        public const string ShopInUser = "ShopInUser";
        public const string EmployeeShopIn = "EmployeeShopIn";

        public static IEnumerable<string> GetRoles() => [
                Administrator,
                Employee,
                EmployeeShopIn,
                DotInSiteUser,
                DotComSiteUser
            ];

        public static Dictionary<string, Dictionary<string, IEnumerable<string>>> GetApplicationPrivileges()
        {
            var privileges = new Dictionary<string, Dictionary<string, IEnumerable<string>>>
            {
                [Administrator] = new Dictionary<string, IEnumerable<string>>
                {
                    { nameof(CountryPolicy), CountryPolicy.GetPolicies() },
                    { nameof(StatePolicy), StatePolicy.GetPolicies() },
                    { nameof(DistrictPolicy), DistrictPolicy.GetPolicies() },
                    { nameof(PostOfficePolicy), PostOfficePolicy.GetPolicies() },
                    { nameof(UserPolicy), UserPolicy.GetPolicies() },
                    { nameof(CategoryPolicy), CategoryPolicy.GetPolicies() },
                    { nameof(SubCategoryPolicy), SubCategoryPolicy.GetPolicies() },
                    { nameof(ShopCategoryPolicy), ShopCategoryPolicy.GetPolicies() }
                },
                [Employee] = new Dictionary<string, IEnumerable<string>>
                {
                    { nameof(PostPolicy), PostPolicy.GetPolicies() },
                    { nameof(StoryPolicy), StoryPolicy.GetPolicies() }
                },
                [EmployeeShopIn] = new Dictionary<string, IEnumerable<string>>
                {
                    { nameof(BlogPolicy), BlogPolicy.GetPolicies() },
                    { nameof(ProductPolicy), ProductPolicy.GetPolicies() }
                }
            };

            return privileges;
        }
    }
}
