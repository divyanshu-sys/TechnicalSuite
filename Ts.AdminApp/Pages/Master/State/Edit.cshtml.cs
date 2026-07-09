using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.StateVms;
namespace Ts.AdminApp.Pages.Master.State
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = StatePolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly IStateClient stateClient;

        public EditModel(IStateClient stateClient)
        {
            this.stateClient = stateClient;
        }

        [BindProperty]
        public UpdateStateVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await stateClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await stateClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "State updated successfully.";
            else
                TempData["fail"] = "Problem in updating State.";
            return RedirectToPage("index");
        }
    }
}
