using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.HomeVms;
namespace Ts.ShopApp.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IHomeClient homeClient;

        public IndexModel(IHomeClient homeClient)
        {
            this.homeClient = homeClient;
        }

        public DisplayHomeItemsVm Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await homeClient.DisplayItemsCacheAsync();
            Model = response.Data;
            return Page();
        }
    }
}
