using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Common.AppInterfaces;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailImageVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanUpdate)]
    public class UpdateProductDetailImageModel : PageModel
    {
        private readonly IProductDetailClient productDetailClient;
        private readonly IFileValidationService fileValidationService;

        public string ImageFileSizeInvalidMsg { get; }
        public string ImageFileExtensionInValidMsg { get; }

        public UpdateProductDetailImageModel(IProductDetailClient productDetailClient,
            IFileValidationService fileValidationService)
        {
            this.productDetailClient = productDetailClient;
            this.fileValidationService = fileValidationService;
            ImageFileSizeInvalidMsg = fileValidationService.ImageFileSizeInvalidMsg;
            ImageFileExtensionInValidMsg = fileValidationService.ImageFileExtensionInValidMsg;
        }

        [BindProperty]
        public UpdateProductDetailImageVm Model { get; set; }

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

            if (!await fileValidationService.ValidateImageFileSizeAsync(Model.ImageFile).ConfigureAwait(false))
            {
                ModelState.AddModelError(string.Empty, fileValidationService.ImageFileSizeInvalidMsg);
                return await GetForEditAsync().ConfigureAwait(false);
            }
            if (!await fileValidationService.ValidateImageFileExtensionAsync(Model.ImageFile).ConfigureAwait(false))
            {
                ModelState.AddModelError(string.Empty, fileValidationService.ImageFileExtensionInValidMsg);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            var response = await productDetailClient.UpdateProductDetailImageAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
            {
                TempData["success"] = "ProductDetail image updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
                TempData["fail"] = "Problem in updating productdetail image.";

            return RedirectToPage("index");
        }
    }
}
