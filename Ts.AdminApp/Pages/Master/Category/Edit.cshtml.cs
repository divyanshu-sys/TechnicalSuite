using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.CategoryVms;
namespace Ts.AdminApp.Pages.Master.Category
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = CategoryPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly ICategoryClient categoryClient;

        public EditModel(ICategoryClient categoryClient)
        {
            this.categoryClient = categoryClient;
        }

        [BindProperty]
        public UpdateCategoryVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await categoryClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await categoryClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Category updated successfully.";
            else
                TempData["fail"] = "Problem in updating category.";
            return RedirectToPage("index");
        }
    }
}
