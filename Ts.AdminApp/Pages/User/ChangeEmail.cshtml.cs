using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
namespace Ts.AdminApp.Pages.User
{
    public class ChangeEmailModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ChangeEmailModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        [BindProperty(SupportsGet = true)]
        public ChangeEmailVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await applicationUserClient.ChangeEmailAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Email sent please verify.";
            else
                TempData["fail"] = "Problem in updating Email.";

            return RedirectToPage("/index");
        }

        public async Task<JsonResult> OnGetIsEmailAvailable()
        {
            var response = await applicationUserClient.IsEmailAvailableAsync(Model.Email).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return new JsonResult(response.ErrorMessage);
            else if (response.Data)
                return new JsonResult(true);
            return new JsonResult("Email already in use.");
        }
    }
}
