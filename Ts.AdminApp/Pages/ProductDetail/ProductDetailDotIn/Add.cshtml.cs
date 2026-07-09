using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanCreate)]
    public class AddModel : PageModel
    {
        private readonly IProductDetailClient productDetailClient;

        public AddModel(IProductDetailClient productDetailClient)
        {
            this.productDetailClient = productDetailClient;
        }

        [BindProperty]
        public CreateProductDetailVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var response = await productDetailClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data != null)
            {
                TempData["success"] = "ProductDetail added successfully.";
                return RedirectToPage("updatedescription", new { id = response.Data.Id });
            }
            else
            {
                TempData["fail"] = "Problem in adding ProductDetail.";
            }

            return RedirectToPage("index");
        }
    }
}
