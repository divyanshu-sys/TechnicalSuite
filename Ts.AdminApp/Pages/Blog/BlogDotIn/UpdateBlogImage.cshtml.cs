using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Common.AppInterfaces;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogImageVms;
using Ts.ShopIn.Client.ViewModels.BlogVms;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanUpdate)]
    public class UpdateBlogImageModel : PageModel
    {
        private readonly IBlogClient blogClient;
        private readonly IFileValidationService fileValidationService;

        public string ImageFileSizeInvalidMsg { get; }
        public string ImageFileExtensionInValidMsg { get; }

        public UpdateBlogImageModel(IBlogClient blogClient,
            IFileValidationService fileValidationService)
        {
            this.blogClient = blogClient;
            this.fileValidationService = fileValidationService;
            ImageFileSizeInvalidMsg = fileValidationService.ImageFileSizeInvalidMsg;
            ImageFileExtensionInValidMsg = fileValidationService.ImageFileExtensionInValidMsg;
        }

        [BindProperty]
        public UpdateBlogImageVm Model { get; set; }

        public BlogVm ModelVm { get; set; }

        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await GetForEditAsync().ConfigureAwait(false);
        }

        private async Task<IActionResult> GetForEditAsync()
        {
            var response = await blogClient.GetForEditAsync(Id).ConfigureAwait(false);

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

            var response = await blogClient.UpdateBlogImageAsync(Model, Id).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                foreach (var error in response.ErrorMessage)
                    ModelState.AddModelError(string.Empty, error);
                return await GetForEditAsync().ConfigureAwait(false);
            }

            if (response.Data)
            {
                TempData["success"] = "Blog image updated successfully.";
                return RedirectToPage(new { Id });
            }
            else
                TempData["fail"] = "Problem in updating blog image.";

            return RedirectToPage("index");
        }
    }
}
