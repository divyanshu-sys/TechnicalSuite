using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Master.PostOffice
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = PostOfficePolicy.CanDelete)]
    [IgnoreAntiforgeryToken]
    public class DeleteModel : PageModel
    {
        private readonly IPostOfficeClient postOfficeClient;

        public DeleteModel(IPostOfficeClient postOfficeClient)
        {
            this.postOfficeClient = postOfficeClient;
        }

        [FromRoute]
        public int Id { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await postOfficeClient.DeleteAsync(Id).ConfigureAwait(false);
            if (response.Data)
                TempData["success"] = "Post Office deleted successfully.";
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
