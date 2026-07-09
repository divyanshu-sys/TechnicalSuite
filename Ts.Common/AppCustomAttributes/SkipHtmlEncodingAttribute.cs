namespace Ts.Common.AppCustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SkipHtmlEncodingAttribute : Attribute
    {
    }
}
