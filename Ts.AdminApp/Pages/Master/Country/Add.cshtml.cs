using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.CountryVms;
namespace Ts.AdminApp.Pages.Master.Country
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = CountryPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly ICountryClient countryClient;

        public AddModel(ICountryClient countryClient)
        {
            this.countryClient = countryClient;
        }

        [BindProperty]
        public CreateCountryVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await countryClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
                TempData["success"] = "Country created successfully.";
            else
                TempData["fail"] = "Problem in creating country.";
            return RedirectToPage("index");
        }
    }
}
