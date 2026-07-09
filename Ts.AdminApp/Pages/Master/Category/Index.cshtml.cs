using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.Master.Category
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = CategoryPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ICategoryClient categoryClient;

        public IndexModel(ICategoryClient categoryClient)
        {
            this.categoryClient = categoryClient;
        }

        [BindProperty]
        public CategoryDataTableRequestVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await categoryClient.GetAllAsync(Model).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
