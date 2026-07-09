using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.StoryVms;
namespace Ts.AdminApp.Pages.Story.StoryDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanPublish)]
    [IgnoreAntiforgeryToken]
    public class PublishModel : PageModel
    {
        private readonly IStoryClient storyClient;

        public PublishModel(IStoryClient storyClient)
        {
            this.storyClient = storyClient;
        }

        public StoryVm Model { get; set; }

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
            var response = await storyClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await storyClient.PublishStoryAsync(Id, IsRepublish ?? false).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
                TempData["success"] = "Story updated successfully.";
            else
                TempData["fail"] = "Problem in updating story.";

            return RedirectToPage("index");
        }
    }
}
