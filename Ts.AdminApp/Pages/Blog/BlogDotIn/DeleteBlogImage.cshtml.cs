using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeleteBlogImageModel : PageModel
    {
        private readonly IBlogClient blogClient;

        public DeleteBlogImageModel(IBlogClient blogClient)
        {
            this.blogClient = blogClient;
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
            var response = await blogClient.DeleteBlogImageAsync(Id, ImageName).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "Blog's image deleted successfully.";
                return RedirectToPage("updateblogimage", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
