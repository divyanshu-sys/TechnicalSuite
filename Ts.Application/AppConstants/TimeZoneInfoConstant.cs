namespace Ts.Application.AppConstants
{
    public static class TimeZoneInfoConstant
    {
        public static readonly TimeZoneInfo India =
            TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public static readonly TimeZoneInfo Pacific =
            TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        public static readonly TimeZoneInfo UnitedKingdom =
            TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
    }
}
