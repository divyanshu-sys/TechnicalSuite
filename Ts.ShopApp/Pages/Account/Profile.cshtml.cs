using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.ViewModels.ClientUserVms;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;

        public ProfileModel(IClientUserClient clientUserClient)
        {
            this.clientUserClient = clientUserClient;
        }

        public ClientUserVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await clientUserClient.GetUserProfileAsync().ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
                return RedirectToPage("/index");
            }

            Model = response.Data;
            return Page();
        }
    }
}
