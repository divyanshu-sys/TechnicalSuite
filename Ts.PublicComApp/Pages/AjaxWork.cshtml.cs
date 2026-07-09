using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Application.Client.HelperExtensions;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos;
namespace Ts.PublicComApp.Pages
{
    [IgnoreAntiforgeryToken]
    public class AjaxWorkModel : PageModel
    {
        private readonly IPostForViewClient postForViewClient;
        private readonly IStoryForViewClient storyForViewClient;

        public AjaxWorkModel(IPostForViewClient postForViewClient,
            IStoryForViewClient storyForViewClient)
        {
            this.postForViewClient = postForViewClient;
            this.storyForViewClient = storyForViewClient;
        }

        [FromQuery]
        public int Start { get; set; }

        [FromQuery]
        public int Length { get; set; }

        [FromQuery]
        public string CategoryName { get; set; }

        [FromQuery]
        public string SubCategoryName { get; set; }

        [FromQuery]
        public string Search { get; set; }

        [FromQuery]
        public int PostId { get; set; }

        [FromQuery]
        public int StoryId { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await postForViewClient.GetForListViewAsync(new PostDataTableForViewRequestDto
            {
                Start = Start,
                Length = Length,
                CategoryName = CategoryName,
                SubCategoryName = SubCategoryName,
                Search = Search
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return new NoContentResult();
            return Partial("_GalleryCasePartial", response.Data);
        }

        public async Task<IActionResult> OnPostStory()
        {
            var response = await storyForViewClient.GetForListViewAsync(new StoryDataTableForViewRequestDto
            {
                Start = Start,
                Length = Length,
                SubCategoryName = SubCategoryName,
                Search = Search
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return new NoContentResult();
            return Partial("_GalleryCaseStoryPartial", response.Data);
        }

        public async Task<IActionResult> OnPostUpdatePostForViewCount()
        {
            var response = await postForViewClient.UpdatePostForViewCountAsync(PostId, Request.Headers.ContainsKey("sec-ch-ua")).ConfigureAwait(false);
            if (response?.Data == true)
                return new NoContentResult();
            if (response == null)
                return BadRequest("Not a browser.");
            return BadRequest(response.ErrorMessage.ToHtmlBreakString());
        }

        public async Task<IActionResult> OnPostUpdateStoryForViewCount()
        {
            var response = await storyForViewClient.UpdateStoryForViewCountAsync(StoryId, Request.Headers.ContainsKey("sec-ch-ua")).ConfigureAwait(false);
            if (response?.Data == true)
                return new NoContentResult();
            if (response == null)
                return BadRequest("Not a browser.");
            return BadRequest(response.ErrorMessage.ToHtmlBreakString());
        }
    }
}
