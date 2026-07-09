using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.StoryVms;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
namespace Ts.PublicApp.Pages.WebStory
{
    public class ListModel : PageModel
    {
        private readonly IStoryForViewClient storyForViewClient;

        public ListModel(IStoryForViewClient storyForViewClient)
        {
            this.storyForViewClient = storyForViewClient;
        }

        [FromRoute]
        public string SubCategoryName { get; set; }

        public IEnumerable<GetStoryForListViewVm> Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await storyForViewClient.GetForListViewAsync(new StoryDataTableForViewRequestDto
            {
                Start = 0,
                Length = 12,
                SubCategoryName = SubCategoryName
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return NotFound();

            Model = response.Data;
            return Page();
        }
    }
}
