using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.PublicApp.Pages
{
    public class SitemapModel : PageModel
    {
        private readonly IPostForViewClient postForViewClient;

        public SitemapModel(IPostForViewClient postForViewClient)
        {
            this.postForViewClient = postForViewClient;
        }
        public async Task<IActionResult> OnGet()
        {
            return Content((await postForViewClient.GetSitemapAsync().ConfigureAwait(false)).Data, "text/xml");
        }
    }
}
