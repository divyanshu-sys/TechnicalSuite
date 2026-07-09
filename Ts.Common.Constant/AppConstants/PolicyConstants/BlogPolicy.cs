namespace Ts.Common.Constant.AppConstants.PolicyConstants
{
    public static class BlogPolicy
    {
        private const string Name = "Blo";

        public const string CanView = "View" + Name;
        public const string CanCreate = "Create" + Name;
        public const string CanUpdate = "Update" + Name;
        public const string CanDelete = "Delete" + Name;
        public const string CanPublish = "Publish" + Name;
        public const string CanChangeWorker = "ChangeWorker" + Name;

        public static IEnumerable<string> GetPolicies() => [
                CanView,
                CanCreate,
                CanUpdate,
                CanDelete,
                CanPublish,
                CanChangeWorker
            ];
    }
}
