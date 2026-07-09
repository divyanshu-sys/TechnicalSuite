using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ShopCategoryVms;
namespace Ts.AdminApp.Pages.Master.ShopCategory
{
    [Authorize(Roles = $"{RoleConstant.Administrator}")]
    [Authorize(Policy = ShopCategoryPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IShopCategoryClient shopCategoryClient;

        public AddModel(IShopCategoryClient shopCategoryClient)
        {
            this.shopCategoryClient = shopCategoryClient;
        }

        [BindProperty]
        public CreateShopCategoryVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await shopCategoryClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
                TempData["success"] = "Shop Category created successfully.";
            else
                TempData["fail"] = "Problem in creating shop category.";
            return RedirectToPage("index");
        }
    }
}
