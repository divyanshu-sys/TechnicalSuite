using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.SubCategoryVms;
namespace Ts.AdminApp.Pages.Master.SubCategory
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = SubCategoryPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly ISubCategoryClient subCategoryClient;

        public EditModel(ISubCategoryClient subCategoryClient)
        {
            this.subCategoryClient = subCategoryClient;
        }

        [BindProperty]
        public UpdateSubCategoryVm Model { get; set; }
        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await subCategoryClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await subCategoryClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Sub Category updated successfully.";
            else
                TempData["fail"] = "Problem in updating sub category.";
            return RedirectToPage("index");
        }
    }
}
