using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Post.PostDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeletePostRelativeModel : PageModel
    {
        private readonly IPostClient postClient;

        public DeletePostRelativeModel(IPostClient postClient)
        {
            this.postClient = postClient;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string HrefLang { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await postClient.DeletePostRelativeAsync(Id, HrefLang).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "Post's relative deleted successfully.";
                return RedirectToPage("updatepostrelative", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
