using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Dto.ApplicationUserDtos;
namespace Ts.AdminApp.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ConfirmEmailModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        [FromQuery]
        public string EncUserId { get; set; }

        [FromQuery]
        public string Token { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(EncUserId) && !string.IsNullOrEmpty(Token))
            {
                var response = await applicationUserClient.ConfirmEmailAsync(new ConfirmEmailDto { EncUserId = EncUserId, Token = Token }).ConfigureAwait(false);
                if (response.Data)
                {
                    TempData["success"] = "Your email confirmed successfully.";
                    return RedirectToPage("index");
                }
            }

            return NotFound();
        }
    }
}
