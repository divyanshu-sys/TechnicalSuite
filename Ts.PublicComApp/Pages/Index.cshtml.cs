using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.PostVms;
using Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos;
namespace Ts.PublicComApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPostForViewClient postForViewClient;

        public IndexModel(IPostForViewClient postForViewClient)
        {
            this.postForViewClient = postForViewClient;
        }

        public IEnumerable<GetPostForListViewVm> Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await postForViewClient.DisplayItemsCacheAsync(new PostDataTableForViewRequestDto
            {
                Start = 0,
                Length = 8
            }).ConfigureAwait(false);
            Model = response.Data;
            return Page();
        }
    }
}
