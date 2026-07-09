using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogVms;
namespace Ts.ShopApp.Pages.Blog
{
    [AllowAnonymous]
    public class ViewModel : PageModel
    {
        private readonly IBlogForViewClient blogForViewClient;

        public ViewModel(IBlogForViewClient blogForViewClient)
        {
            this.blogForViewClient = blogForViewClient;
        }

        [FromRoute]
        public string SubCategoryName { get; set; }

        [FromRoute]
        public string BlogLink { get; set; }

        public BlogVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await blogForViewClient.GetForViewCacheAsync(SubCategoryName, BlogLink).ConfigureAwait(false);
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
