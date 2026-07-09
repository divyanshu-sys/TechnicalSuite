using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Dto.ApplicationUserDtos;
namespace Ts.AdminApp.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ForgotPasswordModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        [BindProperty]
        public ForgotPasswordDto Model { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToPage("/index");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToPage("/index");

            if (!ModelState.IsValid) return Page();
            var response = await applicationUserClient.ForgotPasswordAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "We have sent an email with the instructions to reset your password.";
            else
                TempData["fail"] = "Problem in sending email.";
            return RedirectToPage("index");
        }
    }
}
