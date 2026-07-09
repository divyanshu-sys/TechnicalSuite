using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Common.Helpers;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.Post.PostDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = PostPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IPostClient postClient;
        private readonly ICategoryClient categoryClient;
        private readonly ISubCategoryClient subCategoryClient;
        private readonly IApplicationUserClient applicationUserClient;
        private readonly IDropDownClient dropDownClient;

        public IndexModel(IPostClient postClient, ICategoryClient categoryClient,
            ISubCategoryClient subCategoryClient,
            IApplicationUserClient applicationUserClient, IDropDownClient dropDownClient)
        {
            this.postClient = postClient;
            this.categoryClient = categoryClient;
            this.subCategoryClient = subCategoryClient;
            this.applicationUserClient = applicationUserClient;
            this.dropDownClient = dropDownClient;
        }

        [BindProperty]
        public PostDataTableRequestVm Model { get; set; }

        [FromQuery]
        public bool IsShowAll { get; set; }

        [FromQuery]
        public bool IsPagesVisited { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await postClient.GetAllAsync(Model, IsShowAll, IsPagesVisited).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetCategoryDropDown()
        {
            var response = await categoryClient.GetAllForDropDownAsync().ConfigureAwait(false);
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
            var response = await postClient.GetTotalUniquePagesVisited(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetYearUniquePageViews()
        {
            var range = TimeRangeZoneHelper.GetCurrentYearRange(TimeZoneInfoConstant.India);
            var response = await postClient.GetTotalUniquePagesVisited(range.Start, range.End).ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetTotalPageViews()
        {
            var response = await postClient.GetTotalPagesVisitedLifetime().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }

        public async Task<IActionResult> OnGetHrefLangDropDown()
        {
            var response = await dropDownClient.GetHrefLangAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
