using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels.DataTableVms;
namespace Ts.AdminApp.Pages.Story.StoryDotCom
{
    [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
    [Authorize(Policy = StoryPolicy.CanView)]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IStoryClient storyClient;
        private readonly ICategoryClient categoryClient;
        private readonly ISubCategoryClient subCategoryClient;
        private readonly IApplicationUserClient applicationUserClient;
        private readonly IDropDownClient dropDownClient;

        public IndexModel(IStoryClient storyClient, ICategoryClient categoryClient,
            ISubCategoryClient subCategoryClient,
            IApplicationUserClient applicationUserClient, IDropDownClient dropDownClient)
        {
            this.storyClient = storyClient;
            this.categoryClient = categoryClient;
            this.subCategoryClient = subCategoryClient;
            this.applicationUserClient = applicationUserClient;
            this.dropDownClient = dropDownClient;
        }

        [BindProperty]
        public StoryDataTableRequestVm Model { get; set; }

        [FromQuery]
        public bool IsShowAll { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorMessageHtmlString());
            var response = await storyClient.GetAllAsync(Model, IsShowAll).ConfigureAwait(false);
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

        public async Task<IActionResult> OnGetHrefLangDropDown()
        {
            var response = await dropDownClient.GetHrefLangAsync().ConfigureAwait(false);
            if (response.ErrorMessage.Count != 0)
                return BadRequest(response.ErrorMessage.ToHtmlBreakString());
            return new JsonResult(response.Data);
        }
    }
}
