using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Common.AppInterfaces;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.StoryImageVms;
using Ts.DotIn.Client.ViewModels.StoryVms;
namespace Ts.AdminApp.Pages.Story.StoryDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanUpdate)]
    public class UpdateStoryImageModel : PageModel
    {
        private readonly IStoryClient storyClient;
        private readonly IFileValidationService fileValidationService;

        public string ImageFileSizeInvalidMsg { get; }
        public string ImageFileExtensionInValidMsg { get; }

        public UpdateStoryImageModel(IStoryClient storyClient,
            IFileValidationService fileValidationService)
        {
            this.storyClient = storyClient;
            this.fileValidationService = fileValidationService;
            ImageFileSizeInvalidMsg = fileValidationService.ImageFileSizeInvalidMsg;
            ImageFileExtensionInValidMsg = fileValidationService.ImageFileExtensionInValidMsg;
        }

        [BindProperty]
        public UpdateStoryImageVm Model { get; set; }

        public StoryVm ModelVm { get; set; }

        [FromRoute]
        public int Id { get; set; }

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
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return await GetForEditAsync().ConfigureAwait(false);

            if (!await fileValidationService.ValidateImageFileSizeAsync(Model.ImageFile).ConfigureAwait(false))
            {
                ModelState.AddModelError(string.Empty, fileValidationService.ImageFileSizeInvalidMsg);
                return await GetForEditAsync().ConfigureAwait(false);
            }
            if (!await fileValidationService.ValidateImageFileExtensionAsync(Model.ImageFile).ConfigureAwait(false))
            {
                ModelState.AddModelError(string.Empty, fileValidationService.ImageFileExtensionInValidMsg);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            var response = await storyClient.UpdateStoryImageAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
            {
                TempData["success"] = "Story image updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
                TempData["fail"] = "Problem in updating story image.";

            return RedirectToPage("index");
        }
    }
}
