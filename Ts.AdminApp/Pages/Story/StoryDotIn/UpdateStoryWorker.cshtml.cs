using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.StoryVms;
namespace Ts.AdminApp.Pages.Story.StoryDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanChangeWorker)]
    public class UpdateStoryWorkerModel : PageModel
    {
        private readonly IStoryClient storyClient;

        public UpdateStoryWorkerModel(IStoryClient storyClient)
        {
            this.storyClient = storyClient;
        }

        [BindProperty]
        public UpdateStoryWorkerVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await storyClient.GetForUpdateStoryWorkerAsync(Id).ConfigureAwait(false);

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

            var response = await storyClient.UpdateStoryWorkerAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Story worker updated successfully.";
            else
                TempData["fail"] = "Problem in updating Story worker.";

            return RedirectToPage("index");
        }
    }
}
