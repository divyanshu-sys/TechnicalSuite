using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.StoryVms;
using Ts.Dto;
namespace Ts.AdminApp.Pages.Story.StoryDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class UpdateDescriptionModel : PageModel
    {
        private readonly IStoryClient storyClient;

        public UpdateDescriptionModel(IStoryClient storyClient)
        {
            this.storyClient = storyClient;
        }

        public StoryVm ModelVm { get; set; }

        [BindProperty]
        public UpdateStoryDescriptionVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string DescriptionId { get; set; }

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

            ModelVm = response.Data;

            if (DescriptionId != null)
            {
                Model = storyClient.GetForUpdateDescription(ModelVm, DescriptionId);
                if (Model == null)
                {
                    TempData["fail"] = "Description not found by supplied Description Id";
                    return RedirectToPage(new { Id });
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return await GetForEditAsync().ConfigureAwait(false);

            ResponseMessageDto<bool> response;
            if (DescriptionId == null)
                response = await storyClient.AddDescriptionAsync(Model, Id).ConfigureAwait(false);
            else
                response = await storyClient.UpdateDescriptionAsync(Model, Id, DescriptionId).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
            {
                TempData["success"] = "Story updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
            {
                TempData["fail"] = "Problem in updating Story.";
            }

            return RedirectToPage("index");
        }
    }
}
