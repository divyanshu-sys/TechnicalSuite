using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.BlogVms;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
namespace Ts.ShopApp.Pages.Blog
{
    [AllowAnonymous]
    public class ListModel : PageModel
    {
        private readonly IBlogForViewClient blogForViewClient;

        public ListModel(IBlogForViewClient blogForViewClient)
        {
            this.blogForViewClient = blogForViewClient;
        }

        [FromRoute]
        public string SubCategoryName { get; set; }

        public IEnumerable<GetBlogForListViewVm> Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await blogForViewClient.GetForListViewAsync(new BlogDataTableForViewRequestDto
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
