using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.PostVms;
namespace Ts.AdminApp.Pages.Post.PostDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanChangeWorker)]
    public class UpdatePostWorkerModel : PageModel
    {
        private readonly IPostClient postClient;

        public UpdatePostWorkerModel(IPostClient postClient)
        {
            this.postClient = postClient;
        }

        [BindProperty]
        public UpdatePostWorkerVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await postClient.GetForUpdatePostWorkerAsync(Id).ConfigureAwait(false);

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

            var response = await postClient.UpdatePostWorkerAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Post worker updated successfully.";
            else
                TempData["fail"] = "Problem in updating Post worker.";

            return RedirectToPage("index");
        }
    }
}
