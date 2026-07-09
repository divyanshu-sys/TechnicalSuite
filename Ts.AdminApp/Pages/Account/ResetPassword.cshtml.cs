using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
namespace Ts.AdminApp.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ResetPasswordModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        [FromQuery]
        public string EncUserId { get; set; }
        [FromQuery]
        public string Token { get; set; }

        [BindProperty]
        public ResetPasswordVm Model { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToPage("/index");

            if (string.IsNullOrEmpty(EncUserId) || string.IsNullOrEmpty(Token))
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToPage("/index");

            if (string.IsNullOrEmpty(EncUserId) || string.IsNullOrEmpty(Token))
                return NotFound();

            if (!ModelState.IsValid) return Page();
            var response = await applicationUserClient.ResetPasswordAsync(Model, EncUserId, Token).ConfigureAwait(false);

            if (response.Data)
                TempData["success"] = "Password reset successfully.";
            else
                TempData["fail"] = "Password reset failed.";
            return RedirectToPage("index");
        }
    }
}
