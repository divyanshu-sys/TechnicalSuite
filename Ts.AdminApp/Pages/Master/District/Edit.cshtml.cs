using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DistrictVms;
namespace Ts.AdminApp.Pages.Master.District
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = DistrictPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly IDistrictClient districtClient;

        public EditModel(IDistrictClient districtClient)
        {
            this.districtClient = districtClient;
        }

        [BindProperty]
        public UpdateDistrictVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await districtClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await districtClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "District updated successfully.";
            else
                TempData["fail"] = "Problem in updating District.";
            return RedirectToPage("index");
        }
    }
}
