using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanPublish)]
    [IgnoreAntiforgeryToken]
    public class PublishModel : PageModel
    {
        private readonly IProductDetailClient productdetailClient;

        public PublishModel(IProductDetailClient productdetailClient)
        {
            this.productdetailClient = productdetailClient;
        }

        public ProductDetailVm Model { get; set; }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public bool? IsRepublish { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await GetForEditAsync().ConfigureAwait(false);
        }

        private async Task<IActionResult> GetForEditAsync()
        {
            var response = await productdetailClient.GetForEditAsync(Id).ConfigureAwait(false);

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
            if (!ModelState.IsValid) return await GetForEditAsync().ConfigureAwait(false);

            var response = await productdetailClient.PublishProductDetailAsync(Id, IsRepublish ?? false).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
                TempData["success"] = "ProductDetail updated successfully.";
            else
                TempData["fail"] = "Problem in updating productdetail.";

            return RedirectToPage("index");
        }
    }
}
