using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.Master.State
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = StatePolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IStateClient stateClient;
        private readonly ICountryClient countryClient;

        public IndexModel(IStateClient stateClient, ICountryClient countryClient)
        {
            this.stateClient = stateClient;
            this.countryClient = countryClient;
        }

        [BindProperty]
        public StateDataTableRequestVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await stateClient.GetAllAsync(Model).ConfigureAwait(false);
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
    }
}
