namespace Ts.Common.Constant.AppConstants.PolicyConstants
{
    public static class UserPolicy
    {
        private const string Name = "Use";

        public const string CanView = "View" + Name;
        public const string CanCreate = "Create" + Name;
        public const string CanUpdate = "Update" + Name;
        public const string CanUpdateUserPrivilege = "UpdateUserPrivilege" + Name;
        public const string CanUpdateUserEmail = "UpdateUserEmail" + Name;

        public static IEnumerable<string> GetPolicies() => [
                CanView,
                CanCreate,
                CanUpdate,
                CanUpdateUserPrivilege,
                CanUpdateUserEmail
            ];
    }
}
