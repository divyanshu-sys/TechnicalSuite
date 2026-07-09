using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanUpdate)]
    public class EditModel : PageModel
    {
        private readonly IProductDetailClient productdetailClient;

        public EditModel(IProductDetailClient productdetailClient)
        {
            this.productdetailClient = productdetailClient;
        }

        [BindProperty]
        public UpdateProductDetailVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await productdetailClient.GetForUpdateProductDetailAsync(Id).ConfigureAwait(false);

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

            var response = await productdetailClient.PutAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            if (response.Data)
            {
                TempData["success"] = "ProductDetail updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
            {
                TempData["fail"] = "Problem in updating ProductDetail.";
            }

            return RedirectToPage("index");
        }
    }
}
