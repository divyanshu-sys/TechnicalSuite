using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
namespace Ts.AdminApp.Pages.User
{
    public class ProfileModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ProfileModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        public ApplicationUserVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await applicationUserClient.GetUserProfileAsync().ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
                return RedirectToPage("index");
            }

            Model = response.Data;
            return Page();
        }
    }
}
