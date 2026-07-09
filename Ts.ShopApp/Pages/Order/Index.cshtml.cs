using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;

namespace Ts.ShopApp.Pages.Order
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IOrderForViewClient orderClient;

        public IndexModel(IOrderForViewClient orderClient)
        {
            this.orderClient = orderClient;
        }

        [FromQuery]
        public int Start { get; set; }

        [FromQuery]
        public int Length { get; set; }

        [FromQuery]
        public int OrderDetailId { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var response = await orderClient.GetByUserAsync(new OrderDataTableForViewRequestDto
            {
                Start = Start,
                Length = Length,
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return new NoContentResult();
            return Partial("_OrderListPartial", response.Data);
        }

        public async Task<IActionResult> OnPostDocumentDetail()
        {
            var response = await orderClient.GetDocumentDetailAsync(OrderDetailId).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
