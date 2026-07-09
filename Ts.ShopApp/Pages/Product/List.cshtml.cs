using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.ShopApp.Pages.Product
{
    [AllowAnonymous]
    public class ListModel : PageModel
    {
        private readonly IProductDetailForViewClient productDetailForViewClient;

        public ListModel(IProductDetailForViewClient productDetailForViewClient)
        {
            this.productDetailForViewClient = productDetailForViewClient;
        }

        [FromRoute]
        public string ShopCategoryName { get; set; }

        public IEnumerable<GetProductDetailForListViewVm> Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await productDetailForViewClient.GetForListViewAsync(new()
            {
                Start = 0,
                Length = 12,
                ShopCategoryName = ShopCategoryName
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return NotFound();

            Model = response.Data;
            return Page();
        }
    }
}
