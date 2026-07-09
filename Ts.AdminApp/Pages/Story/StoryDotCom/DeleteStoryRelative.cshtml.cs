using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Story.StoryDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeleteStoryRelativeModel : PageModel
    {
        private readonly IStoryClient storyClient;

        public DeleteStoryRelativeModel(IStoryClient storyClient)
        {
            this.storyClient = storyClient;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string HrefLang { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await storyClient.DeleteStoryRelativeAsync(Id, HrefLang).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "Story's relative deleted successfully.";
                return RedirectToPage("updatestoryrelative", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
