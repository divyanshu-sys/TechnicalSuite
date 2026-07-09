using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogVms;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IBlogClient blogClient;

        public AddModel(IBlogClient blogClient)
        {
            this.blogClient = blogClient;
        }

        [BindProperty]
        public CreateBlogVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await blogClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
            {
                TempData["success"] = "Blog added successfully.";
                return RedirectToPage("updatedescription", new { id = response.Data.Id });
            }
            else
            {
                TempData["fail"] = "Problem in adding Blog.";
            }

            return RedirectToPage("index");
        }
    }
}
