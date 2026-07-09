using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Application.Client.AppConstants;
using Ts.Application.Client.AppCustomAttributes;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ClientUserVms;
namespace Ts.ShopApp.Pages.Account
{
    [AllowAnonymous]
    [RazorValidateGoogleReCaptcha(GoogleCaptchaConstant.GoogleCaptchaShopInAppSiteKey,
        GoogleCaptchaConstant.GoogleCaptchaRegister)]
    public class RegisterModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;

        public RegisterModel(IClientUserClient clientUserClient)
        {
            this.clientUserClient = clientUserClient;
        }

        [BindProperty(SupportsGet = true)]
        public RegisterClientUserVm Model { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToPage("/index");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await clientUserClient.RegisterUserAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "User created successfully. Verification link sent to your email. Please check spam folder if not found in inbox.";
            else
                TempData["fail"] = "Problem in creating user.";

            return RedirectToPage("index");
        }

        public async Task<JsonResult> OnGetIsEmailAvailable()
        {
            var response = await clientUserClient.IsEmailAvailableAsync(Model.Email).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return new JsonResult(response.ErrorMessage);
            else if (response.Data)
                return new JsonResult(true);
            return new JsonResult("Email already in use");
        }
    }
}
