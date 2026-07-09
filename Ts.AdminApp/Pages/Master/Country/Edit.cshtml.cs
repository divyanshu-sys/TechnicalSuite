using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.CountryVms;
namespace Ts.AdminApp.Pages.Master.Country
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = CountryPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly ICountryClient countryClient;

        public EditModel(ICountryClient countryClient)
        {
            this.countryClient = countryClient;
        }

        [BindProperty]
        public UpdateCountryVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await countryClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await countryClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Country updated successfully.";
            else
                TempData["fail"] = "Problem in updating country.";
            return RedirectToPage("index");
        }
    }
}
