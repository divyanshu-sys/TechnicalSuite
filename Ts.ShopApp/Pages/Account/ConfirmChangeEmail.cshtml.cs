using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Dto.ApplicationUserDtos;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmChangeEmailModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;

        public ConfirmChangeEmailModel(IClientUserClient clientUserClient)
        {
            this.clientUserClient = clientUserClient;
        }

        [FromQuery]
        public string EncUserId { get; set; }

        [FromQuery]
        public string EncEmail { get; set; }

        [FromQuery]
        public string Token { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(EncEmail) && !string.IsNullOrEmpty(Token))
            {
                var response = await clientUserClient.ConfirmChangeEmailAsync(new ConfirmChangeEmailDto { EncUserId = EncUserId, EncEmail = EncEmail, Token = Token }).ConfigureAwait(false);
                if (response.Data)
                {
                    TempData["success"] = "Your email changed successfully.";
                    return RedirectToPage("/index");
                }
            }

            return NotFound();
        }
    }
}
