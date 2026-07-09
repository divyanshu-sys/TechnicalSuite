namespace Ts.Common.Constant.AppConstants.PolicyConstants
{
    public static class DistrictPolicy
    {
        private const string Name = "Dis";

        public const string CanView = "View" + Name;
        public const string CanCreate = "Create" + Name;
        public const string CanUpdate = "Update" + Name;
        public const string CanDelete = "Delete" + Name;

        public static IEnumerable<string> GetPolicies() => [
                CanView,
                CanCreate,
                CanUpdate,
                CanDelete
            ];
    }
}
