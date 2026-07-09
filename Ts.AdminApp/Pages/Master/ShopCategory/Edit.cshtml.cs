using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ShopCategoryVms;
namespace Ts.AdminApp.Pages.Master.ShopCategory
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = ShopCategoryPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly IShopCategoryClient shopCategoryClient;

        public EditModel(IShopCategoryClient shopCategoryClient)
        {
            this.shopCategoryClient = shopCategoryClient;
        }

        [BindProperty]
        public UpdateShopCategoryVm Model { get; set; }
        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await shopCategoryClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await shopCategoryClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
                TempData["success"] = "Shop Category updated successfully.";
            else
                TempData["fail"] = "Problem in updating shop category.";
            return RedirectToPage("index");
        }
    }
}
