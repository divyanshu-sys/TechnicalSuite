using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Post.PostDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeletePostImageModel : PageModel
    {
        private readonly IPostClient postClient;

        public DeletePostImageModel(IPostClient postClient)
        {
            this.postClient = postClient;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string ImageName { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await postClient.DeletePostImageAsync(Id, ImageName).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "Post's image deleted successfully.";
                return RedirectToPage("updatepostimage", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
