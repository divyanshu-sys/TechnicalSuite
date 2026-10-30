using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.DataTableVms;

namespace Ts.AdminApp.Pages.Order.OrderDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IOrderClient orderClient;

        public IndexModel(IOrderClient orderClient)
        {
            this.orderClient = orderClient;
        }

        [BindProperty]
        public OrderDataTableRequestVm Model { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await orderClient.GetAllAsync(Model);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
