using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.SubCategoryVms;
namespace Ts.AdminApp.Pages.Master.SubCategory
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = SubCategoryPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly ISubCategoryClient subCategoryClient;

        public AddModel(ISubCategoryClient subCategoryClient)
        {
            this.subCategoryClient = subCategoryClient;
        }

        [BindProperty]
        public CreateSubCategoryVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await subCategoryClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
                TempData["success"] = "Sub Category created successfully.";
            else
                TempData["fail"] = "Problem in creating sub category.";
            return RedirectToPage("index");
        }
    }
}
