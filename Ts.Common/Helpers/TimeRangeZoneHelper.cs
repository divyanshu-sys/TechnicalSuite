namespace Ts.Common.Helpers
{
    public static class TimeRangeZoneHelper
    {
        public static (string Start, string End) GetTodayRange(TimeZoneInfo timeZone)
        {
            var nowInZone = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

            // Local midnight in that timezone
            var startLocal = nowInZone.Date; // DateTime (Kind = Unspecified)

            var start = new DateTimeOffset(
                startLocal,
                timeZone.GetUtcOffset(startLocal));

            var end = start.AddDays(1); // exclusive

            return (start.ToString("o"), end.ToString("o"));
        }

        public static (string Start, string End) GetCurrentYearRange(TimeZoneInfo timeZone)
        {
            var nowInZone = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

            var startLocal = new DateTime(
                nowInZone.Year,
                1,
                1,
                0, 0, 0,
                DateTimeKind.Unspecified);

            var start = new DateTimeOffset(
                startLocal,
                timeZone.GetUtcOffset(startLocal));

            var end = start.AddYears(1); // exclusive

            return (start.ToString("o"), end.ToString("o"));
        }

        public static (string Start, string End) GetCurrentMonthRange(TimeZoneInfo timeZone)
        {
            var nowInZone = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

            var startLocal = new DateTime(
                nowInZone.Year,
                nowInZone.Month,
                1,
                0, 0, 0,
                DateTimeKind.Unspecified);

            var start = new DateTimeOffset(
                startLocal,
                timeZone.GetUtcOffset(startLocal));

            var end = start.AddMonths(1);

            return (start.ToString("o"), end.ToString("o"));
        }

        public static (string Start, string End) GetFinancialYearRange(TimeZoneInfo timeZone)
        {
            var nowInZone = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

            // Financial year starts on April 1
            int financialYearStart =
                nowInZone.Month >= 4
                    ? nowInZone.Year
                    : nowInZone.Year - 1;

            var startLocal = new DateTime(
                financialYearStart,
                4,
                1,
                0, 0, 0,
                DateTimeKind.Unspecified);

            var start = new DateTimeOffset(
                startLocal,
                timeZone.GetUtcOffset(startLocal));

            var end = start.AddYears(1);

            return (start.ToString("o"), end.ToString("o"));
        }
    }
}
