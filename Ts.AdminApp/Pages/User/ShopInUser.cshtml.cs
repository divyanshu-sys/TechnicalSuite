using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.User
{
    [Authorize(Roles = RoleConstant.Administrator)]
    [Authorize(Policy = UserPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class ShopInModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public ShopInModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        [BindProperty]
        public ClientUserDataTableRequestVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await applicationUserClient.GetAllShopInUserAsync(Model).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
