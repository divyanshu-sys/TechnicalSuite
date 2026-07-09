using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.PostOfficeVms;
namespace Ts.AdminApp.Pages.Master.PostOffice
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = PostOfficePolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IPostOfficeClient postOfficeClient;

        public AddModel(IPostOfficeClient postOfficeClient)
        {
            this.postOfficeClient = postOfficeClient;
        }

        [BindProperty]
        public CreatePostOfficeVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await postOfficeClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
                TempData["success"] = "PostOffice created successfully.";
            else
                TempData["fail"] = "Problem in creating PostOffice.";
            return RedirectToPage("index");
        }
    }
}
