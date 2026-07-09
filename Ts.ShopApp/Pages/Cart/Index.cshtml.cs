using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Dto.CartDtos;
using Ts.ShopIn.Dto.PaymentDtos;
namespace Ts.ShopApp.Pages.Cart
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ICartClient cartClient;
        private readonly IOrderForViewClient orderClient;

        public IndexModel(ICartClient cartClient, IOrderForViewClient orderClient)
        {
            this.cartClient = cartClient;
            this.orderClient = orderClient;
        }

        public GetCartForListViewDto ModelVm { get; set; }

        [BindProperty]
        public VerifyRazorpayPaymentDto Model { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await cartClient.GetByUserAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                TempData["fail"] = response.ErrorMessage.ToHtmlBreakString();
            else
                ModelVm = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostCheckout()
        {
            var response = await orderClient.CreateRazorpayOrderAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnPostVerifyOrderPayment()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await orderClient.VerifyRazorpayOrderPaymentAsync(Model).ConfigureAwait(false);
            if (response.Data)
                return new NoContentResult();
            return BadRequest(response.ErrorMessage.ToHtmlBreakString());
        }
    }
}
