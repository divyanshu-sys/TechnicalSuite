using System.ComponentModel.DataAnnotations;
namespace Ts.Common.AppCustomAttributes
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute(string minimumValue) : base(typeof(DateTime), minimumValue, "31-Dec-9999 11:59:59 PM")
        {

        }
    }
}
