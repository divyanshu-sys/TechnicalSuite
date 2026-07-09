using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotCom.Dto.PostDtos;
using Ts.DotCom.Dto.PostImageDtos;
using Ts.DotCom.Dto.PostRelativeDtos;
using Ts.DotCom.Service.DataInterfaces;
using Ts.Dto;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class PostDotComController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IPostViewCounter postViewCounter;

        public PostDotComController(IPostService postService, IPostViewCounter postViewCounter)
        {
            this.postService = postService;
            this.postViewCounter = postViewCounter;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanView)]
        [HttpPost("datatable/{isShowAll}")]
        [ProducesResponseType(typeof(DataTableResponseDto<GetPostDataTableDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(PostDataTableRequestDto modelDto, bool isShowAll)
        {
            if (isShowAll)
                return Ok(await postService.GetAllAsync(modelDto).ConfigureAwait(false));
            return Ok(await postService.GetAllAsync(modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreatePostDto modelDto)
        {
            var responseResult = await postService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(GetForEdit), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanView)]
        [HttpGet("{id:min(1)}/for-edit")]
        [ProducesResponseType(typeof(GetUpdatePostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var responseResult = await postService.GetForEditAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdatePostDto modelDto)
        {
            var responseResult = await postService.UpdateAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanDelete)]
        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int id)
        {
            var responseResult = await postService.DeleteAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanUpdate)]
        [HttpPatch("{id:min(1)}/description")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PatchDescription(int id, UpdatePostDescriptionDto modelDto)
        {
            var responseResult = await postService.UpdateDescriptionAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanChangeWorker)]
        [HttpPut("{id:min(1)}/postworker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdatePostWorker(int id, UpdatePostWorkerDto modelDto)
        {
            var responseResult = await postService.UpdatePostWorkerAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/main-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateMainImage(int id, UpdatePostMainImageDto modelDto)
        {
            var responseResult = await postService.UpdateMainImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/post-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdatePostImage(int id, UpdatePostImageDto modelDto)
        {
            var responseResult = await postService.UpdatePostImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/post-image/{imageName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeletePostImage(int id, string imageName)
        {
            var responseResult = await postService.DeletePostImageAsync(id, imageName, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanPublish)]
        [HttpPut("{id:min(1)}/publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Publish(int id, PublishPostDto modelDto)
        {
            var responseResult = await postService.PublishPostAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.DotComSiteUser}")]
        [HttpPost("post-list-for-view")]
        [ProducesResponseType(typeof(IEnumerable<GetPostForListViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForListView(PostDataTableForViewRequestDto modelDto)
        {
            return Ok(await postService.GetForListViewAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.DotComSiteUser}")]
        [HttpGet("post-for-view/{categoryName}/{subCategoryName}/{postLink}")]
        [ProducesResponseType(typeof(GetPostForViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForView(string categoryName, string subCategoryName, string postLink)
        {
            var responseResult = await postService.GetForViewAsync(categoryName, subCategoryName, postLink).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.DotComSiteUser}")]
        [HttpPost("post-for-view-count/{postId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePostForViewCount(int postId)
        {
            _ = postViewCounter.IncrementAsync(postId).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(Roles = $"{RoleConstant.DotComSiteUser}")]
        [HttpGet("sitemap")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSitemap()
        {
            return Ok(await postService.GetSitemapAsync().ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanView)]
        [HttpGet("total-unique-pages-visited")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalUniquePagesVisited(DateTimeOffset? lastViewedOnStart, DateTimeOffset? lastViewedOnEnd)
        {
            var response = await postService.GetTotalUniquePagesVisitedAsync(lastViewedOnStart, lastViewedOnEnd).ConfigureAwait(false);
            return Ok(response);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanView)]
        [HttpGet("total-pages-visited-lifetime")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPagesVisitedLifetime()
        {
            var response = await postService.GetTotalPagesVisitedLifetimeAsync().ConfigureAwait(false);
            return Ok(response);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/post-relative")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdatePostRelative(int id, UpdatePostRelativeDto modelDto)
        {
            var responseResult = await postService.UpdatePostRelativeAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = PostPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/post-relative/{hrefLang}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeletePostRelative(int id, string hrefLang)
        {
            var responseResult = await postService.DeletePostRelativeAsync(id, hrefLang, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }
    }
}
