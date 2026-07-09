using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Story.StoryDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeleteStoryImageModel : PageModel
    {
        private readonly IStoryClient storyClient;

        public DeleteStoryImageModel(IStoryClient storyClient)
        {
            this.storyClient = storyClient;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string ImageName { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await storyClient.DeleteStoryImageAsync(Id, ImageName).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "Story's image deleted successfully.";
                return RedirectToPage("updatestoryimage", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
