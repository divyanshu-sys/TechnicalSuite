using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shyjus.BrowserDetection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ts.Application.Client.AppConstants;
using Ts.Application.Client.AppCustomAttributes;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages.Account
{
    [AllowAnonymous]
    [RazorValidateGoogleReCaptcha(GoogleCaptchaConstant.GoogleCaptchaShopInAppSiteKey,
        GoogleCaptchaConstant.GoogleCaptchaLogin)]
    public class IndexModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;
        private readonly IBrowserDetector browserDetector;

        public IndexModel(IClientUserClient clientUserClient, IBrowserDetector browserDetector)
        {
            this.clientUserClient = clientUserClient;
            this.browserDetector = browserDetector;
        }

        [BindProperty]
        public LoginVm Model { get; set; }

        [FromQuery]
        public string ReturnUrl { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);
                return RedirectToPage("/index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);
                return RedirectToPage("/index");
            }

            if (!ModelState.IsValid) return Page();

            var response = await clientUserClient.LoginAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.ReadToken(response.Data.ApiToken) is JwtSecurityToken payload)
            {
                var claimsIdentity = new ClaimsIdentity(payload.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var browserClaims = new List<Claim>
                {
                    new("devicetype", browserDetector.Browser.DeviceType),
                    new("browsername", browserDetector.Browser.Name),
                    new("browseros", browserDetector.Browser.OS),
                    new("refreshtoken", response.Data.RefreshEncodedToken),
                    new("authenticationscheme", response.Data.AuthenticationScheme),
                    new("ispersistent", Model.RememberMe.ToString())
                };
                var browserIdentity = new ClaimsIdentity(browserClaims, "Browser Identity");

                var tokenClaims = new List<Claim>
                {
                    new("token", response.Data.ApiToken)
                };
                var tokenIdentity = new ClaimsIdentity(tokenClaims, "Token Identity");

                var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity, browserIdentity, tokenIdentity });

                var properties = new AuthenticationProperties
                {
                    IsPersistent = Model.RememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties).ConfigureAwait(false);
                TempData["success"] = "Welcome to Shop.TechnicalSuite.in";
            }
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return Redirect(ReturnUrl);
            return RedirectToPage("/index");
        }
    }
}
