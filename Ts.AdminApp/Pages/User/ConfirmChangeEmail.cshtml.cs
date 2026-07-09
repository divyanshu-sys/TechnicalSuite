using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Dto.ApplicationUserDtos;
namespace Ts.AdminApp.Pages.User
{
    [AllowAnonymous]
    public class ConfirmChangeEmailModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ConfirmChangeEmailModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
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
                var response = await applicationUserClient.ConfirmChangeEmailAsync(new ConfirmChangeEmailDto { EncUserId = EncUserId, EncEmail = EncEmail, Token = Token }).ConfigureAwait(false);
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
