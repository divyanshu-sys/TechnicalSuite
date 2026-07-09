using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.StateVms;
namespace Ts.AdminApp.Pages.Master.State
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = StatePolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IStateClient stateClient;

        public AddModel(IStateClient stateClient)
        {
            this.stateClient = stateClient;
        }

        [BindProperty]
        public CreateStateVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await stateClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
                TempData["success"] = "State created successfully.";
            else
                TempData["fail"] = "Problem in creating State.";
            return RedirectToPage("index");
        }
    }
}
