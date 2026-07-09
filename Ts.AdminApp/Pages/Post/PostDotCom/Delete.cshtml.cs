using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Post.PostDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanDelete)]
    [IgnoreAntiforgeryToken]
    public class DeleteModel : PageModel
    {
        private readonly IPostClient postClient;

        public DeleteModel(IPostClient postClient)
        {
            this.postClient = postClient;
        }

        [FromRoute]
        public int Id { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await postClient.DeleteAsync(Id).ConfigureAwait(false);
            if (response.Data)
                TempData["success"] = "Post deleted successfully.";
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
