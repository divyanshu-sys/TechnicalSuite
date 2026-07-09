using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages.Account
{
    public class ChangeEmailModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;

        public ChangeEmailModel(IClientUserClient clientUserClient)
        {
            this.clientUserClient = clientUserClient;
        }

        [BindProperty(SupportsGet = true)]
        public ChangeEmailVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await clientUserClient.ChangeEmailAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Email sent please verify. Please check spam folder if not found in inbox.";
            else
                TempData["fail"] = "Problem in updating Email.";

            return RedirectToPage("profile");
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
