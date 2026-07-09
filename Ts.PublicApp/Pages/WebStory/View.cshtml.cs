using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.StoryVms;
namespace Ts.PublicApp.Pages.WebStory
{
    public class ViewModel : PageModel
    {
        private readonly IStoryForViewClient storyForViewClient;

        public ViewModel(IStoryForViewClient storyForViewClient)
        {
            this.storyForViewClient = storyForViewClient;
        }

        [FromRoute]
        public string SubCategoryName { get; set; }

        [FromRoute]
        public string StoryLink { get; set; }

        public StoryVm Model { get; set; }
        public string Keywords { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await storyForViewClient.GetForViewCacheAsync(SubCategoryName, StoryLink).ConfigureAwait(false);
            if (response.Data == null)
                return RedirectToPage("/webstory/list");

            Model = response.Data;

            var sb = new StringBuilder(Model.Keyword1);
            sb.Append("," + Model.Keyword2);
            if (!string.IsNullOrEmpty(Model.Keyword3))
                sb.Append("," + Model.Keyword3);
            if (!string.IsNullOrEmpty(Model.Keyword4))
                sb.Append("," + Model.Keyword4);
            if (!string.IsNullOrEmpty(Model.Keyword5))
                sb.Append("," + Model.Keyword5);
            Keywords = sb.ToString();

            return Page();
        }
    }
}
