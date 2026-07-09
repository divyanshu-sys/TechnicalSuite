namespace Ts.Common.Constant.AppConstants.PolicyConstants
{
    public static class CategoryPolicy
    {
        private const string Name = "Cat";

        public const string CanView = "View" + Name;
        public const string CanCreate = "Create" + Name;
        public const string CanUpdate = "Update" + Name;

        public static IEnumerable<string> GetPolicies() => [
                CanView,
                CanCreate,
                CanUpdate
            ];
    }
}
