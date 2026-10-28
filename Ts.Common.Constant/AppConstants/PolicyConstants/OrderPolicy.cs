namespace Ts.Common.Constant.AppConstants.PolicyConstants
{
    public static class OrderPolicy
    {
        private const string Name = "Ord";

        public const string CanView = "View" + Name;
        public const string CanUpdate = "Update" + Name;

        public static IEnumerable<string> GetPolicies() => [
                CanView,
                CanUpdate
            ];
    }
}
