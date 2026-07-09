using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ClientUserVms;
namespace Ts.ShopApp.Pages.Account
{
    public class EditProfileModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;

        public EditProfileModel(IClientUserClient clientUserClient)
        {
            this.clientUserClient = clientUserClient;
        }

        [BindProperty(SupportsGet = true)]
        public UpdateClientUserVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await clientUserClient.GetForEditAsync().ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
                return RedirectToPage("profile");
            }

            Model = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await clientUserClient.UpdateForEditAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "User updated successfully.";
            else
                TempData["fail"] = "Problem in updating User.";

            return RedirectToPage("profile");
        }
    }
}
