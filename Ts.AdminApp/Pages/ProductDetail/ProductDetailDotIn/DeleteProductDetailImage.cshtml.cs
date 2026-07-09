using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeleteProductDetailImageModel : PageModel
    {
        private readonly IProductDetailClient productdetailClient;

        public DeleteProductDetailImageModel(IProductDetailClient productdetailClient)
        {
            this.productdetailClient = productdetailClient;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string ImageName { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await productdetailClient.DeleteProductDetailImageAsync(Id, ImageName).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "ProductDetail's image deleted successfully.";
                return RedirectToPage("updateproductdetailimage", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
