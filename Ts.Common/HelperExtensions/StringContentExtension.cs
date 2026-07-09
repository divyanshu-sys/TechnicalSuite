using Newtonsoft.Json;
using System.Net.Http.Headers;
namespace Ts.Common.HelperExtensions
{
    public static class StringContentExtension
    {
        public static StringContent ToJsonStringContent(this object model)
        {
            var jsonContent = JsonConvert.SerializeObject(model, Formatting.Indented);
            var content = new StringContent(jsonContent);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
