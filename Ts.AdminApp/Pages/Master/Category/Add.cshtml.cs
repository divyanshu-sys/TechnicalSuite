using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.CategoryVms;
namespace Ts.AdminApp.Pages.Master.Category
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = CategoryPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly ICategoryClient categoryClient;

        public AddModel(ICategoryClient categoryClient)
        {
            this.categoryClient = categoryClient;
        }

        [BindProperty]
        public CreateCategoryVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await categoryClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
                TempData["success"] = "Category created successfully.";
            else
                TempData["fail"] = "Problem in creating category.";
            return RedirectToPage("index");
        }
    }
}
