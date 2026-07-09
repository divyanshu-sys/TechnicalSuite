using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.PostVms;
namespace Ts.AdminApp.Pages.Post.PostDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IPostClient postClient;

        public AddModel(IPostClient postClient)
        {
            this.postClient = postClient;
        }

        [BindProperty]
        public CreatePostVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await postClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
            {
                TempData["success"] = "Post added successfully.";
                return RedirectToPage("updatedescription", new { id = response.Data.Id });
            }
            else
            {
                TempData["fail"] = "Problem in adding Post.";
            }

            return RedirectToPage("index");
        }
    }
}
