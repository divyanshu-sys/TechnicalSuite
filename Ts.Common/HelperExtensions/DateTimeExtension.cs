namespace Ts.Common.HelperExtensions
{
    public static class DateTimeExtension
    {
        public static string ToDateTimeIstString(this DateTime utc) => utc.AddHours(5.5).ToString("dd-MM-yyyy hh:mm tt \"IST\"");

        public static string ToDateIstString(this DateTime utc) => utc.AddHours(5.5).ToString("dd-MM-yyyy\"-IST\"");

        public static string ToDateTimeCstString(this DateTime utc) => utc.AddHours(-6).ToString("dd-MM-yyyy hh:mm tt \"CST\"");
        public static string ToDateTimeUtcString(this DateTime utc) => utc.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}
