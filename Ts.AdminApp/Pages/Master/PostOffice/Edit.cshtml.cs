using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.PostOfficeVms;
namespace Ts.AdminApp.Pages.Master.PostOffice
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = PostOfficePolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly IPostOfficeClient postOfficeClient;

        public EditModel(IPostOfficeClient postOfficeClient)
        {
            this.postOfficeClient = postOfficeClient;
        }

        [BindProperty]
        public UpdatePostOfficeVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await postOfficeClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await postOfficeClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "PostOffice updated successfully.";
            else
                TempData["fail"] = "Problem in updating PostOffice.";
            return RedirectToPage("index");
        }
    }
}
