using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;

        public ResetPasswordModel(IClientUserClient clientUserClient)
        {
            this.clientUserClient = clientUserClient;
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
            var response = await clientUserClient.ResetPasswordAsync(Model, EncUserId, Token).ConfigureAwait(false);

            if (response.Data)
                TempData["success"] = "Password reset successfully.";
            else
                TempData["fail"] = "Password reset failed.";
            return RedirectToPage("index");
        }
    }
}
