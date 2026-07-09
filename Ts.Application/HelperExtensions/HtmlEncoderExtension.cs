using System.Reflection;
using System.Text.Encodings.Web;
using Ts.Common.AppCustomAttributes;
namespace Ts.Application.HelperExtensions
{
    public static class HtmlEncoderExtension
    {
        public static void HtmlEncodeObject(this object obj)
        {
            EncodeOperation(obj);
        }

        private static void EncodeOperation(object obj)
        {
            if (obj == null)
                return;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) && property.CanWrite)
                {
                    var value = property.GetValue(obj);
                    if (value != null)
                    {
                        var str = Convert.ToString(value).Trim();
                        if (ShouldSkipEncoding(property))
                            property.SetValue(obj, str);
                        else
                            property.SetValue(obj, HtmlEncoder.Default.Encode(str));
                    }
                }
                else if (typeof(IEnumerable<object>).IsAssignableFrom(property.PropertyType))
                {
                    var collection = (IEnumerable<object>)property.GetValue(obj);
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            item.HtmlEncodeObject();
                        }
                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    property.GetValue(obj).HtmlEncodeObject();
                }
            }
        }

        private static bool ShouldSkipEncoding(PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(SkipHtmlEncodingAttribute), true).Any();
        }
    }
}
