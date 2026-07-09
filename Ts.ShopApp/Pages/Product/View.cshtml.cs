using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.ShopApp.Pages.Product
{
    [AllowAnonymous]
    public class ViewModel : PageModel
    {
        private readonly IProductDetailForViewClient productDetailForViewClient;

        public ViewModel(IProductDetailForViewClient productDetailForViewClient)
        {
            this.productDetailForViewClient = productDetailForViewClient;
        }

        [FromRoute]
        public string ShopCategoryName { get; set; }

        [FromRoute]
        public string ProductDetailLink { get; set; }

        public ProductDetailVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await productDetailForViewClient.GetForViewCacheAsync(ShopCategoryName, ProductDetailLink).ConfigureAwait(false);
            if (response.Data == null)
                return NotFound();

            Model = response.Data;

            var sb = new StringBuilder(Model.Keyword1);
            sb.Append("," + Model.Keyword2);
            if (!string.IsNullOrEmpty(Model.Keyword3))
                sb.Append("," + Model.Keyword3);
            if (!string.IsNullOrEmpty(Model.Keyword4))
                sb.Append("," + Model.Keyword4);
            if (!string.IsNullOrEmpty(Model.Keyword5))
                sb.Append("," + Model.Keyword5);
            ViewData["Keywords"] = sb.ToString();

            return Page();
        }
    }
}
