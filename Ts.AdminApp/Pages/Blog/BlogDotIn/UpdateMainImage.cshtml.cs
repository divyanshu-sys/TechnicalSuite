using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogVms;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanUpdate)]
    public class UpdateMainImageModel : PageModel
    {
        private readonly IBlogClient blogClient;

        public UpdateMainImageModel(IBlogClient blogClient)
        {
            this.blogClient = blogClient;
        }

        [BindProperty]
        public UpdateBlogMainImageVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await blogClient.GetForUpdateMainImageAsync(Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
                return RedirectToPage("index");
            }

            Model = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await blogClient.UpdateMainImageAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
            {
                TempData["success"] = "Blog image updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
                TempData["fail"] = "Problem in updating Blog image.";

            return RedirectToPage("index");
        }
    }
}
