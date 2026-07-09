using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.Master.Country
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = CountryPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ICountryClient countryClient;

        public IndexModel(ICountryClient countryClient)
        {
            this.countryClient = countryClient;
        }

        [BindProperty]
        public CountryDataTableRequestVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await countryClient.GetAllAsync(Model).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
