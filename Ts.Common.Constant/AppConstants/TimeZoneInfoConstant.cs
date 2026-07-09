namespace Ts.Common.Constant.AppConstants
{
    public static class TimeZoneInfoConstant
    {
        public static readonly TimeZoneInfo India =
            TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public static readonly TimeZoneInfo Pacific =
            TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        public static readonly TimeZoneInfo UnitedKingdom =
            TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        public static readonly string[] MonthNames = {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };
    }
}
