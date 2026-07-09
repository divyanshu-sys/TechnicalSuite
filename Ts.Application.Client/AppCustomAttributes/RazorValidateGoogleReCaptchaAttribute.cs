using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;
using Ts.Application.Client.AppConstants;
namespace Ts.Application.Client.AppCustomAttributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RazorValidateGoogleReCaptchaAttribute : Attribute, IAsyncPageFilter
    {
        private readonly string _siteKey;
        private readonly string _action;

        public RazorValidateGoogleReCaptchaAttribute(string siteKey, string action)
        {
            _siteKey = siteKey;
            _action = action;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            await next.Invoke().ConfigureAwait(false);
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            var env = context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
                return;

            if (context.HandlerMethod.MethodInfo.Name == "OnPost")
            {
                var httpClient = context.HttpContext.RequestServices.GetRequiredService<HttpClient>();

                string reCaptcha = context.HttpContext.Request.Form["ReCaptcha"];

                if (!string.IsNullOrEmpty(reCaptcha))
                {
                    try
                    {
                        var requestData = new ReCaptchaRequest
                        {
                            Event = new()
                            {
                                Token = reCaptcha,
                                ExpectedAction = _action,
                                SiteKey = _siteKey
                            }
                        };
                        var response = await httpClient.PostAsync(GoogleCaptchaConstant.GoogleCaptchaUrl, new StringContent(JsonConvert.SerializeObject(requestData))).ConfigureAwait(false);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var responseData = JsonConvert.DeserializeObject<ReCaptchaResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                            if (responseData == null || !responseData.TokenProperties.Valid || responseData.RiskAnalysis.Score < 0.7f)
                                context.ModelState.AddModelError(string.Empty, "Invalid request.");
                        }
                        else
                            context.ModelState.AddModelError(string.Empty, "Invalid request. Captcha response failed.");
                    }
                    catch
                    {
                        context.ModelState.AddModelError(string.Empty, "Error occurred while validating Captcha.");
                    }
                }
                else
                    context.ModelState.AddModelError(string.Empty, "Invalid request. Captcha not found.");
            }
        }
    }
}
