using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DistrictVms;
namespace Ts.AdminApp.Pages.Master.District
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = DistrictPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IDistrictClient districtClient;

        public AddModel(IDistrictClient districtClient)
        {
            this.districtClient = districtClient;
        }

        [BindProperty]
        public CreateDistrictVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await districtClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
                TempData["success"] = "District created successfully.";
            else
                TempData["fail"] = "Problem in creating District.";
            return RedirectToPage("index");
        }
    }
}
