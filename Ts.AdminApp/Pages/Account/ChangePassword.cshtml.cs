using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
namespace Ts.AdminApp.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ChangePasswordModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        [BindProperty]
        public ChangePasswordVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (Model.CurrentPassword == Model.Password)
                ModelState.AddModelError(string.Empty, "Current Password and New Password cannot be same.");

            if (!ModelState.IsValid) return Page();

            var response = await applicationUserClient.ChangePasswordAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
            {
                TempData["success"] = "Password changed successfully.";
                await HttpContext.SignOutAsync().ConfigureAwait(false);
                return RedirectToPage("index");
            }

            TempData["fail"] = "Password changed failed.";
            return RedirectToPage("/index");
        }
    }
}
