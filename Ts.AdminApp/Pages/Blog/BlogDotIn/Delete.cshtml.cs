using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanDelete)]
    [IgnoreAntiforgeryToken]
    public class DeleteModel : PageModel
    {
        private readonly IBlogClient blogClient;

        public DeleteModel(IBlogClient blogClient)
        {
            this.blogClient = blogClient;
        }

        [FromRoute]
        public int Id { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await blogClient.DeleteAsync(Id).ConfigureAwait(false);
            if (response.Data)
                TempData["success"] = "Blog deleted successfully.";
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
