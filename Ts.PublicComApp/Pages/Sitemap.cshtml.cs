using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
namespace Ts.PublicComApp.Pages
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
