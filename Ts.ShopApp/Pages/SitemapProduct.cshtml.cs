using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages
{
    [AllowAnonymous]
    public class SitemapProductModel : PageModel
    {
        private readonly IProductDetailForViewClient productDetailForViewClient;

        public SitemapProductModel(IProductDetailForViewClient productDetailForViewClient)
        {
            this.productDetailForViewClient = productDetailForViewClient;
        }
        public async Task<IActionResult> OnGet()
        {
            return Content((await productDetailForViewClient.GetSitemapAsync().ConfigureAwait(false)).Data, "text/xml");
        }
    }
}
