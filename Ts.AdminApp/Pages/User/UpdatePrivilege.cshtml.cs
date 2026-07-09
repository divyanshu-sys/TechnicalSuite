using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
namespace Ts.AdminApp.Pages.User
{
    [Authorize(Roles = RoleConstant.Administrator)]
    [Authorize(Policy = UserPolicy.CanUpdateUserPrivilege)]
    public class UpdatePrivilegeModel : PageModel
    {
        private readonly IApplicationUserClient applicationUserClient;

        public UpdatePrivilegeModel(IApplicationUserClient applicationUserClient)
        {
            this.applicationUserClient = applicationUserClient;
        }

        [FromRoute]
        public string Id { get; set; }

        [BindProperty]
        public UpdatePrivilegeVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (Id == null)
                return NotFound();
            return await GetUserPrivileges().ConfigureAwait(false);

        }

        private async Task<IActionResult> GetUserPrivileges()
        {
            var response = await applicationUserClient.GetUserPrivilegeAsync(Id).ConfigureAwait(false);

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
            if (!ModelState.IsValid) return await GetUserPrivileges().ConfigureAwait(false);

            var response = await applicationUserClient.UpdateUserPrivilegeAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetUserPrivileges().ConfigureAwait(false);
            }

            if (response.Data)
            {
                TempData["success"] = "User privileges updated successfully.";
                if (Id == User.Claims.GetUserId())
                    await HttpContext.SignOutAsync().ConfigureAwait(false);
            }
            else
                TempData["fail"] = "Problem in updating user privilege.";

            return RedirectToPage("index");
        }
    }
}
