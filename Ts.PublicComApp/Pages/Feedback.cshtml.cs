using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels;
namespace Ts.PublicComApp.Pages
{
    public class FeedbackModel : PageModel
    {
        private readonly IContactClient contactClient;

        public FeedbackModel(IContactClient contactClient)
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
