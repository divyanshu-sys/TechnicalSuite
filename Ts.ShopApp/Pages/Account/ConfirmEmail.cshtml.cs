using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Dto.ApplicationUserDtos;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly IClientUserClient clientUserClient;

        public ConfirmEmailModel(IClientUserClient clientUserClient)
        {
            this.clientUserClient = clientUserClient;
        }

        [FromQuery]
        public string EncUserId { get; set; }

        [FromQuery]
        public string Token { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(EncUserId) && !string.IsNullOrEmpty(Token))
            {
                var response = await clientUserClient.ConfirmEmailAsync(new ConfirmEmailDto { EncUserId = EncUserId, Token = Token }).ConfigureAwait(false);
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
