using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.Master.District
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = DistrictPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IDistrictClient districtClient;
        private readonly ICountryClient countryClient;
        private readonly IStateClient stateClient;

        public IndexModel(IDistrictClient districtClient,
            ICountryClient countryClient,
            IStateClient stateClient)
        {
            this.districtClient = districtClient;
            this.countryClient = countryClient;
            this.stateClient = stateClient;
        }

        [BindProperty]
        public DistrictDataTableRequestVm Model { get; set; }

        [FromQuery]
        public int CountryId { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await districtClient.GetAllAsync(Model).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
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
    }
}
