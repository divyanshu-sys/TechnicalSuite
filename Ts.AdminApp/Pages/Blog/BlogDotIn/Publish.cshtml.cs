using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogVms;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanPublish)]
    [IgnoreAntiforgeryToken]
    public class PublishModel : PageModel
    {
        private readonly IBlogClient blogClient;

        public PublishModel(IBlogClient blogClient)
        {
            this.blogClient = blogClient;
        }

        public BlogVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public bool? IsRepublish { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await GetForEditAsync().ConfigureAwait(false);
        }

        private async Task<IActionResult> GetForEditAsync()
        {
            var response = await blogClient.GetForEditAsync(Id).ConfigureAwait(false);

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
            if (!ModelState.IsValid) return await GetForEditAsync().ConfigureAwait(false);

            var response = await blogClient.PublishBlogAsync(Id, IsRepublish ?? false).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
                TempData["success"] = "Blog updated successfully.";
            else
                TempData["fail"] = "Problem in updating blog.";

            return RedirectToPage("index");
        }
    }
}
