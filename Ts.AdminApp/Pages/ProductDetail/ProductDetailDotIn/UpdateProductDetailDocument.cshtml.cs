using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Common.AppInterfaces;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailDocumentVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanUpdate)]
    public class UpdateProductDetailDocumentModel : PageModel
    {
        private readonly IProductDetailClient productDetailClient;
        private readonly IFileValidationService fileValidationService;

        public string DocumentFileSizeInvalidMsg { get; }
        public string DocumentFileExtensionInValidMsg { get; }
        public string ImageFileExtensionInValidMsg { get; }

        public UpdateProductDetailDocumentModel(IProductDetailClient productDetailClient,
            IFileValidationService fileValidationService)
        {
            this.productDetailClient = productDetailClient;
            this.fileValidationService = fileValidationService;
            DocumentFileSizeInvalidMsg = fileValidationService.DocumentFileSizeInvalidMsg;
            DocumentFileExtensionInValidMsg = fileValidationService.DocumentFileExtensionInValidMsg;
            ImageFileExtensionInValidMsg = fileValidationService.ImageFileExtensionInValidMsg;
        }

        [BindProperty]
        public UpdateProductDetailDocumentVm Model { get; set; }

        public ProductDetailVm ModelVm { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await GetForEditAsync().ConfigureAwait(false);
        }

        private async Task<IActionResult> GetForEditAsync()
        {
            var response = await productDetailClient.GetForEditAsync(Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
                return RedirectToPage("index");
            }

            ModelVm = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return await GetForEditAsync().ConfigureAwait(false);

            if (!await fileValidationService.ValidateDocumentFileSizeAsync(Model.DocumentFile).ConfigureAwait(false))
            {
                ModelState.AddModelError(string.Empty, fileValidationService.DocumentFileSizeInvalidMsg);
                return await GetForEditAsync().ConfigureAwait(false);
            }
            if (!await fileValidationService.ValidateDocumentFileExtensionAsync(Model.DocumentFile).ConfigureAwait(false))
            {
                ModelState.AddModelError(string.Empty, fileValidationService.DocumentFileExtensionInValidMsg);
                ModelState.AddModelError(string.Empty, fileValidationService.ImageFileExtensionInValidMsg);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            var response = await productDetailClient.UpdateProductDetailDocumentAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
            {
                TempData["success"] = "ProductDetail document updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
                TempData["fail"] = "Problem in updating productdetail document.";

            return RedirectToPage("index");
        }
    }
}
