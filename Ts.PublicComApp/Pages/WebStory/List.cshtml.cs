using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.StoryVms;
using Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos;
namespace Ts.PublicComApp.Pages.WebStory
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
