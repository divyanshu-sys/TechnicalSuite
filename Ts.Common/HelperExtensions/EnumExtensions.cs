using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace Ts.Common.HelperExtensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetCustomAttribute<DisplayAttribute>()?.GetName();
        }
    }
}
