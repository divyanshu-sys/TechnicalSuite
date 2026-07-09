using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Dto.PaymentDtos;

namespace Ts.ShopApp.Pages.Order
{
    public class PaymentStatusModel : PageModel
    {
        private readonly IOrderForViewClient orderClient;

        public PaymentStatusModel(IOrderForViewClient orderClient)
        {
            this.orderClient = orderClient;
        }

        [FromQuery]
        public string PaymentId { get; set; }

        public GetPaymentStatusDto ModelVm { get; set; }
        public List<string> ErrorsVm { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await orderClient.GetPaymentStatusAsync(PaymentId).ConfigureAwait(false);

            if (response.ErrorMessage.Count != 0)
            {
                ErrorsVm = response.ErrorMessage;
                return Page();
            }

            ModelVm = response.Data;
            return Page();
        }
    }
}
