using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.PostVms;
namespace Ts.AdminApp.Pages.Post.PostDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanPublish)]
    [IgnoreAntiforgeryToken]
    public class PublishModel : PageModel
    {
        private readonly IPostClient postClient;

        public PublishModel(IPostClient postClient)
        {
            this.postClient = postClient;
        }

        public PostVm Model { get; set; }

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
            var response = await postClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await postClient.PublishPostAsync(Id, IsRepublish ?? false).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
                TempData["success"] = "Post updated successfully.";
            else
                TempData["fail"] = "Problem in updating post.";

            return RedirectToPage("index");
        }
    }
}
