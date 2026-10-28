using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.StoryRelativeVms;
using Ts.DotCom.Client.ViewModels.StoryVms;
namespace Ts.AdminApp.Pages.Story.StoryDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanUpdate)]
    public class UpdateStoryRelativeModel : PageModel
    {
        private readonly IStoryClient storyClient;

        public UpdateStoryRelativeModel(IStoryClient storyClient)
        {
            this.storyClient = storyClient;
        }

        [BindProperty]
        public UpdateStoryRelativeVm Model { get; set; }

        public StoryVm ModelVm { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await GetForEditAsync();
        }

        private async Task<IActionResult> GetForEditAsync()
        {
            var response = await storyClient.GetForEditAsync(Id);

            if (response.ErrorMessage.Count != 0)
            {
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
                return RedirectToPage("index");
            }

            ModelVm = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return await GetForEditAsync();

            var response = await storyClient.UpdateStoryRelativeAsync(Model, Id);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync();
            }

            if (response.Data)
            {
                TempData["success"] = "Story relative updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
                TempData["fail"] = "Problem in updating story relative.";

            return RedirectToPage("index");
        }
    }
}
