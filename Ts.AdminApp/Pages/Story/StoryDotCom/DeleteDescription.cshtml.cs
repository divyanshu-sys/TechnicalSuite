using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Story.StoryDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeleteDescriptionModel : PageModel
    {
        private readonly IStoryClient storyClient;

        public DeleteDescriptionModel(IStoryClient storyClient)
        {
            this.storyClient = storyClient;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string DescriptionId { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await storyClient.DeleteDescriptionAsync(Id, DescriptionId).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "Story's description deleted successfully.";
                return RedirectToPage("updatedescription", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
