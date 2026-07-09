using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeleteProductDetailDocumentModel : PageModel
    {
        private readonly IProductDetailClient productdetailClient;

        public DeleteProductDetailDocumentModel(IProductDetailClient productdetailClient)
        {
            this.productdetailClient = productdetailClient;
        }

        [FromRoute]
        public int Id { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await productdetailClient.DeleteProductDetailDocumentAsync(Id).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "ProductDetail's document deleted successfully.";
                return RedirectToPage("updateproductdetaildocument", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
