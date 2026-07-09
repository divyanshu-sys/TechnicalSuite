using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels;
namespace Ts.ShopApp.Pages
{
    [AllowAnonymous]
    public class ContactModel : PageModel
    {
        private readonly IContactClient contactClient;

        public ContactModel(IContactClient contactClient)
        {
            this.contactClient = contactClient;
        }

        [BindProperty]
        public ContactFormVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await contactClient.ContactAsync(Model).ConfigureAwait(false);

            if (response.Data)
            {
                TempData["success"] = "Message sent successfully.";
                return RedirectToPage();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Problem in sending message.");
            }
            return Page();
        }
    }
}
