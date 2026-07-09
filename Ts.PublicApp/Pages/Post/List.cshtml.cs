using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.ViewModels.PostVms;
using Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos;
namespace Ts.PublicApp.Pages.Post
{
    public class ListModel : PageModel
    {
        private readonly IPostForViewClient postForViewClient;

        public ListModel(IPostForViewClient postForViewClient)
        {
            this.postForViewClient = postForViewClient;
        }

        [FromRoute]
        public string CategoryName { get; set; }

        [FromRoute]
        public string SubCategoryName { get; set; }

        public IEnumerable<GetPostForListViewVm> Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await postForViewClient.GetForListViewAsync(new PostDataTableForViewRequestDto
            {
                Start = 0,
                Length = 12,
                CategoryName = CategoryName,
                SubCategoryName = SubCategoryName
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return NotFound();

            Model = response.Data;
            return Page();
        }
    }
}
