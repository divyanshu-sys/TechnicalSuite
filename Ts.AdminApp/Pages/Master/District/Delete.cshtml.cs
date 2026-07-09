using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
namespace Ts.AdminApp.Pages.Master.District
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = DistrictPolicy.CanDelete)]
    [IgnoreAntiforgeryToken]
    public class DeleteModel : PageModel
    {
        private readonly IDistrictClient districtClient;

        public DeleteModel(IDistrictClient districtClient)
        {
            this.districtClient = districtClient;
        }

        [FromRoute]
        public int Id { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await districtClient.DeleteAsync(Id).ConfigureAwait(false);
            if (response.Data)
                TempData["success"] = "District deleted successfully.";
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
