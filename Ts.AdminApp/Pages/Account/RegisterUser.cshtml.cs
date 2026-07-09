using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
namespace Ts.AdminApp.Pages.Account
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = UserPolicy.CanCreate)]
    public class RegisterUserModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;
        private readonly ICountryClient countryClient;
        private readonly IStateClient stateClient;
        private readonly IDistrictClient districtClient;
        private readonly IPostOfficeClient postOfficeClient;

        public RegisterUserModel(IApplicationUserClient applicationUserClient,
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
        public RegisterApplicationUserVm Model { get; set; }

        [FromQuery]
        public int CountryId { get; set; }

        [FromQuery]
        public int StateId { get; set; }

        [FromQuery]
        public int DistrictId { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await applicationUserClient.RegisterUserAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "User created successfully. Please add roles and privileges.";
            else
                TempData["fail"] = "Problem in creating user.";

            return RedirectToPage("/user/index");
        }

        public async Task<JsonResult> OnGetIsEmailAvailable()
        {
            var response = await applicationUserClient.IsEmailAvailableAsync(Model.Email).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return new JsonResult(response.ErrorMessage);
            else if (response.Data)
                return new JsonResult(true);
            return new JsonResult("Email already in use.");
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
