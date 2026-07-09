using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogVms;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class UpdateDescriptionModel : PageModel
    {
        private readonly IBlogClient blogClient;

        public UpdateDescriptionModel(IBlogClient blogClient)
        {
            this.blogClient = blogClient;
        }

        [BindProperty]
        public UpdateBlogDescriptionVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await blogClient.GetForUpdateDescriptionAsync(Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
                return RedirectToPage("index");
            }

            Model = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostByAjax()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());

            var response = await blogClient.UpdateDescriptionAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());

            return new JsonResult(response.Data);
        }
    }
}
