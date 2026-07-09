using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Common.Helpers;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.DataTableVms;

namespace Ts.AdminApp.Pages.ProductDetail.ProductDetailDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = ProductPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IProductDetailClient productdetailClient;
        private readonly IShopCategoryClient shopCategoryClient;
        private readonly IApplicationUserClient applicationUserClient;
        private readonly IDropDownClient dropDownClient;
        private readonly IProductClient productClient;
        private readonly IOrderClient orderClient;

        public IndexModel(IProductDetailClient productdetailClient, IShopCategoryClient shopCategoryClient,
            IApplicationUserClient applicationUserClient, IDropDownClient dropDownClient, IProductClient productClient,
            IOrderClient orderClient)
        {
            this.productdetailClient = productdetailClient;
            this.shopCategoryClient = shopCategoryClient;
            this.applicationUserClient = applicationUserClient;
            this.dropDownClient = dropDownClient;
            this.productClient = productClient;
            this.orderClient = orderClient;
        }

        [BindProperty]
        public ProductDetailDataTableRequestVm Model { get; set; }

        [FromQuery]
        public bool IsShowAll { get; set; }

        [FromQuery]
        public bool IsPagesVisited { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await productdetailClient.GetAllAsync(Model, IsShowAll, IsPagesVisited).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetShopCategoryDropDown()
        {
            var response = await shopCategoryClient.GetAllForDropDownAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetProductDropDown()
        {
            var response = await productClient.GetAllForDropDownAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetExchangePolicyDropDown()
        {
            var response = await dropDownClient.GetExchangePolicyDropDownAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetDeliveryPolicyDropDown()
        {
            var response = await dropDownClient.GetDeliveryPolicyDropDownAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetReturnPolicyDropDown()
        {
            var response = await dropDownClient.GetReturnPolicyDropDownAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetApplicationUserDropDown()
        {
            var response = await applicationUserClient.GetAllForDropDownAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetTodayUniquePageViews()
        {
            var range = TimeRangeZoneHelper.GetTodayRange(TimeZoneInfoConstant.India);
            var response = await productdetailClient.GetTotalUniquePagesVisited(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetYearUniquePageViews()
        {
            var range = TimeRangeZoneHelper.GetCurrentYearRange(TimeZoneInfoConstant.India);
            var response = await productdetailClient.GetTotalUniquePagesVisited(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetTotalPageViews()
        {
            var response = await productdetailClient.GetTotalPagesVisitedLifetime().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetTodayRevenue()
        {
            var range = TimeRangeZoneHelper.GetTodayRange(TimeZoneInfoConstant.India);
            var response = await orderClient.GetTotalRevenue(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetMonthRevenue()
        {
            var range = TimeRangeZoneHelper.GetCurrentMonthRange(TimeZoneInfoConstant.India);
            var response = await orderClient.GetTotalRevenue(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetFinYearRevenue()
        {
            var range = TimeRangeZoneHelper.GetFinancialYearRange(TimeZoneInfoConstant.India);
            var response = await orderClient.GetTotalRevenue(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetFinYearSales()
        {
            var range = TimeRangeZoneHelper.GetFinancialYearRange(TimeZoneInfoConstant.India);
            var response = await orderClient.GetSalesData(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
