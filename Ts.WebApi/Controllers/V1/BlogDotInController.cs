using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.BlogImageDtos;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class BlogDotInController : ControllerBase
    {
        private readonly IBlogService blogService;
        private readonly IBlogViewCounter blogViewCounter;

        public BlogDotInController(IBlogService blogService, IBlogViewCounter blogViewCounter)
        {
            this.blogService = blogService;
            this.blogViewCounter = blogViewCounter;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanView)]
        [HttpPost("datatable/{isShowAll}")]
        [ProducesResponseType(typeof(DataTableResponseDto<GetBlogDataTableDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(BlogDataTableRequestDto modelDto, bool isShowAll)
        {
            if (isShowAll)
                return Ok(await blogService.GetAllAsync(modelDto).ConfigureAwait(false));
            return Ok(await blogService.GetAllAsync(modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateBlogDto modelDto)
        {
            var responseResult = await blogService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(GetForEdit), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanView)]
        [HttpGet("{id:min(1)}/for-edit")]
        [ProducesResponseType(typeof(GetUpdateBlogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var responseResult = await blogService.GetForEditAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdateBlogDto modelDto)
        {
            var responseResult = await blogService.UpdateAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanDelete)]
        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int id)
        {
            var responseResult = await blogService.DeleteAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanUpdate)]
        [HttpPatch("{id:min(1)}/description")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PatchDescription(int id, UpdateBlogDescriptionDto modelDto)
        {
            var responseResult = await blogService.UpdateDescriptionAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanChangeWorker)]
        [HttpPut("{id:min(1)}/blogworker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateBlogWorker(int id, UpdateBlogWorkerDto modelDto)
        {
            var responseResult = await blogService.UpdateBlogWorkerAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/main-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateMainImage(int id, UpdateBlogMainImageDto modelDto)
        {
            var responseResult = await blogService.UpdateMainImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/blog-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateBlogImage(int id, UpdateBlogImageDto modelDto)
        {
            var responseResult = await blogService.UpdateBlogImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/blog-image/{imageName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteBlogImage(int id, string imageName)
        {
            var responseResult = await blogService.DeleteBlogImageAsync(id, imageName, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanPublish)]
        [HttpPut("{id:min(1)}/publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Publish(int id, PublishBlogDto modelDto)
        {
            var responseResult = await blogService.PublishBlogAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpPost("blog-list-for-view")]
        [ProducesResponseType(typeof(IEnumerable<GetBlogForListViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForListView(BlogDataTableForViewRequestDto modelDto)
        {
            return Ok(await blogService.GetForListViewAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpGet("blog-for-view/{subCategoryName}/{blogLink}")]
        [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForView(string subCategoryName, string blogLink)
        {
            var responseResult = await blogService.GetForViewAsync(subCategoryName, blogLink).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpPost("blog-for-view-count/{blogId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateBlogForViewCount(int blogId)
        {
            _ = blogViewCounter.IncrementAsync(blogId).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpGet("sitemap")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSitemap()
        {
            return Ok(await blogService.GetSitemapAsync().ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanView)]
        [HttpGet("total-unique-pages-visited")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalUniquePagesVisited(DateTimeOffset? lastViewedOnStart, DateTimeOffset? lastViewedOnEnd)
        {
            var response = await blogService.GetTotalUniquePagesVisitedAsync(lastViewedOnStart, lastViewedOnEnd).ConfigureAwait(false);
            return Ok(response);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = BlogPolicy.CanView)]
        [HttpGet("total-pages-visited-lifetime")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPagesVisitedLifetime()
        {
            var response = await blogService.GetTotalPagesVisitedLifetimeAsync().ConfigureAwait(false);
            return Ok(response);
        }
    }
}
