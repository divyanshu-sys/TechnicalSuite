using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages
{
    [AllowAnonymous]
    public class SitemapModel : PageModel
    {
        private readonly IBlogForViewClient blogForViewClient;

        public SitemapModel(IBlogForViewClient blogForViewClient)
        {
            this.blogForViewClient = blogForViewClient;
        }
        public async Task<IActionResult> OnGet()
        {
            return Content((await blogForViewClient.GetSitemapAsync().ConfigureAwait(false)).Data, "text/xml");
        }
    }
}
