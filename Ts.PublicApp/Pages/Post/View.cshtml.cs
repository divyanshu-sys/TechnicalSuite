using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.PostVms;
namespace Ts.PublicApp.Pages.Post
{
    public class ViewModel : PageModel
    {
        private readonly IPostForViewClient postForViewClient;

        public ViewModel(IPostForViewClient postForViewClient)
        {
            this.postForViewClient = postForViewClient;
        }

        [FromRoute]
        public string CategoryName { get; set; }

        [FromRoute]
        public string SubCategoryName { get; set; }

        [FromRoute]
        public string PostLink { get; set; }

        public PostVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await postForViewClient.GetForViewCacheAsync(CategoryName, SubCategoryName, PostLink).ConfigureAwait(false);
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
