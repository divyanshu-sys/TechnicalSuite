using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Common.Helpers;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.Blog.BlogDotIn
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
    [Authorize(Policy = BlogPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IBlogClient blogClient;
        private readonly ISubCategoryClient subCategoryClient;
        private readonly IApplicationUserClient applicationUserClient;

        public IndexModel(IBlogClient blogClient, ISubCategoryClient subCategoryClient,
            IApplicationUserClient applicationUserClient)
        {
            this.blogClient = blogClient;
            this.subCategoryClient = subCategoryClient;
            this.applicationUserClient = applicationUserClient;
        }

        [BindProperty]
        public BlogDataTableRequestVm Model { get; set; }

        [FromQuery]
        public bool IsShowAll { get; set; }

        [FromQuery]
        public bool IsPagesVisited { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await blogClient.GetAllAsync(Model, IsShowAll, IsPagesVisited).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetSubCategoryDropDown()
        {
            var response = await subCategoryClient.GetAllForDropDownAsync().ConfigureAwait(false);
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
            var response = await blogClient.GetTotalUniquePagesVisited(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetYearUniquePageViews()
        {
            var range = TimeRangeZoneHelper.GetCurrentYearRange(TimeZoneInfoConstant.India);
            var response = await blogClient.GetTotalUniquePagesVisited(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetTotalPageViews()
        {
            var response = await blogClient.GetTotalPagesVisitedLifetime().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
