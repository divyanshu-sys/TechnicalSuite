using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.PostVms;
using Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos;
namespace Ts.PublicApp.Pages.Post
{
    public class SearchModel : PageModel
    {
        private readonly IPostForViewClient postForViewClient;

        public SearchModel(IPostForViewClient postForViewClient)
        {
            this.postForViewClient = postForViewClient;
        }

        [FromQuery]
        public string Q { get; set; }

        public IEnumerable<GetPostForListViewVm> Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await postForViewClient.GetForListViewAsync(new PostDataTableForViewRequestDto
            {
                Start = 0,
                Length = 12,
                Search = Q
            }).ConfigureAwait(false);

            Model = response.Data;
            return Page();
        }
    }
}
