using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Master.Country
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = CountryPolicy.CanDelete)]
    [IgnoreAntiforgeryToken]
    public class DeleteModel : PageModel
    {
        private readonly ICountryClient countryClient;

        public DeleteModel(ICountryClient countryClient)
        {
            this.countryClient = countryClient;
        }

        [FromRoute]
        public int Id { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await countryClient.DeleteAsync(Id).ConfigureAwait(false);
            if (response.Data)
                TempData["success"] = "Country deleted successfully.";
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
