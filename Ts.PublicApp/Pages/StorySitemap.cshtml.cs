using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.PublicApp.Pages
{
    public class StorySitemapModel : PageModel
    {
        private readonly IStoryForViewClient storyForViewClient;

        public StorySitemapModel(IStoryForViewClient storyForViewClient)
        {
            this.storyForViewClient = storyForViewClient;
        }
        public async Task<IActionResult> OnGet()
        {
            return Content((await storyForViewClient.GetSitemapAsync().ConfigureAwait(false)).Data, "text/xml");
        }
    }
}
