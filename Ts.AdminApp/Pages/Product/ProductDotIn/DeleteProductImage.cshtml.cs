using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;

namespace Ts.AdminApp.Pages.Product.ProductDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanUpdate)]
    [IgnoreAntiforgeryToken]
    public class DeleteProductImageModel : PageModel
    {
        private readonly IProductClient productClient;

        public DeleteProductImageModel(IProductClient productClient)
        {
            this.productClient = productClient;
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
            var response = await productClient.DeleteProductImageAsync(Id, ImageName).ConfigureAwait(false);
            if (response.Data)
            {
                TempData["success"] = "Product's image deleted successfully.";
                return RedirectToPage("updateproductimage", new { Id });
            }
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
