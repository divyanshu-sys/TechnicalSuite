using Newtonsoft.Json;
namespace Ts.Application.Client.AppCustomAttributes
{
    public class ReCaptchaRequest
    {
        [JsonProperty("event")]
        public ReCaptchaEvent Event { get; set; }
    }

    public class ReCaptchaEvent
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expectedAction")]
        public string ExpectedAction { get; set; }

        [JsonProperty("siteKey")]
        public string SiteKey { get; set; }
    }
}
