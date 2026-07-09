using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;

namespace Ts.ShopApp.Pages.Cart
{
    [IgnoreAntiforgeryToken]
    public class DeleteModel : PageModel
    {
        private readonly ICartClient cartClient;

        public DeleteModel(ICartClient cartClient)
        {
            this.cartClient = cartClient;
        }

        [FromRoute]
        public int ProductDetailId { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await cartClient.DeleteAsync(ProductDetailId).ConfigureAwait(false);
            if (response.Data)
                TempData["success"] = "Item removed successfully.";
            else
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();

            return RedirectToPage("index");
        }
    }
}
