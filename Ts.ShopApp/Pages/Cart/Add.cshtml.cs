using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.CartVms;

namespace Ts.ShopApp.Pages.Cart
{
    [IgnoreAntiforgeryToken]
    public class AddModel : PageModel
    {
        private readonly ICartClient cartClient;

        public AddModel(ICartClient cartClient)
        {
            this.cartClient = cartClient;
        }

        [BindProperty]
        public CreateCartVm Model { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());

            var response = await cartClient.PostAsync(Model).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());

            return new NoContentResult();
        }
    }
}
