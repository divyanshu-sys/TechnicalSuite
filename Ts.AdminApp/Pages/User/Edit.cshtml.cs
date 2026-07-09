using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
namespace Ts.AdminApp.Pages.User
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = UserPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;
        private readonly ICountryClient countryClient;
        private readonly IStateClient stateClient;
        private readonly IDistrictClient districtClient;
        private readonly IPostOfficeClient postOfficeClient;

        public EditModel(IApplicationUserClient applicationUserClient,
            ICountryClient countryClient,
            IStateClient stateClient,
            IDistrictClient districtClient,
            IPostOfficeClient postOfficeClient)
        {
            this.applicationUserClient = applicationUserClient;
            this.countryClient = countryClient;
            this.stateClient = stateClient;
            this.districtClient = districtClient;
            this.postOfficeClient = postOfficeClient;
        }

        [BindProperty(SupportsGet = true)]
        public UpdateApplicationUserVm Model { get; set; }

        [FromRoute]
        public string Id { get; set; }

        [FromQuery]
        public int CountryId { get; set; }

        [FromQuery]
        public int StateId { get; set; }

        [FromQuery]
        public int DistrictId { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (Id == null)
                return NotFound();
            var response = await applicationUserClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await applicationUserClient.UpdateForEditAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "User updated successfully.";
            else
                TempData["fail"] = "Problem in updating User.";

            return RedirectToPage("index");
        }

        public async Task<IActionResult> OnGetCountryDropDown()
        {
            var response = await countryClient.GetAllForDropDownAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetStateDropDownByCountryId()
        {
            var response = await stateClient.GetAllForDropDownByCountryIdAsync(CountryId).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetDistrictDropDownByStateId()
        {
            var response = await districtClient.GetAllForDropDownByStateIdAsync(StateId).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetPostOfficeDropDownByDistrictId()
        {
            var response = await postOfficeClient.GetAllForDropDownByDistrictIdAsync(DistrictId).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
