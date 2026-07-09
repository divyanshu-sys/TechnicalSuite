using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductVms;

namespace Ts.AdminApp.Pages.Product.ProductDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IProductClient productClient;

        public AddModel(IProductClient productClient)
        {
            this.productClient = productClient;
        }

        [BindProperty]
        public CreateProductVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await productClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
            {
                TempData["success"] = "Product added successfully.";
                return RedirectToPage("updatemainimage", new { id = response.Data.Id });
            }
            else
            {
                TempData["fail"] = "Problem in adding Product.";
            }

            return RedirectToPage("index");
        }
    }
}
