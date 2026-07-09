using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.ShopApp.Pages.Product
{
    [AllowAnonymous]
    public class SearchModel : PageModel
    {
        private readonly IProductDetailForViewClient productDetailForViewClient;

        public SearchModel(IProductDetailForViewClient productDetailForViewClient)
        {
            this.productDetailForViewClient = productDetailForViewClient;
        }

        [FromQuery]
        public string Q { get; set; }

        public IEnumerable<GetProductDetailForListViewVm> Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await productDetailForViewClient.GetForListViewAsync(new()
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
