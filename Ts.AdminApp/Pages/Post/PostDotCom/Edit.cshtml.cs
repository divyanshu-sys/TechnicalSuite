using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.PostVms;
namespace Ts.AdminApp.Pages.Post.PostDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly IPostClient postClient;

        public EditModel(IPostClient postClient)
        {
            this.postClient = postClient;
        }

        [BindProperty]
        public UpdatePostVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await postClient.GetForUpdatePostAsync(Id).ConfigureAwait(false);

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

            var response = await postClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
            {
                TempData["success"] = "Post updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
            {
                TempData["fail"] = "Problem in updating Post.";
            }

            return RedirectToPage("index");
        }
    }
}
