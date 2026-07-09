using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotIn.Dto.StoryDtos;
using Ts.DotIn.Dto.StoryImageDtos;
using Ts.DotIn.Dto.StoryRelativeDtos;
using Ts.DotIn.Service.DataInterfaces;
using Ts.Dto;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class StoryDotInController : ControllerBase
    {
        private readonly IStoryService storyService;
        private readonly IStoryViewCounter storyViewCounter;

        public StoryDotInController(IStoryService storyService, IStoryViewCounter storyViewCounter)
        {
            this.storyService = storyService;
            this.storyViewCounter = storyViewCounter;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanView)]
        [HttpPost("datatable/{isShowAll}")]
        [ProducesResponseType(typeof(DataTableResponseDto<GetStoryDataTableDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(StoryDataTableRequestDto modelDto, bool isShowAll)
        {
            if (isShowAll)
                return Ok(await storyService.GetAllAsync(modelDto).ConfigureAwait(false));
            return Ok(await storyService.GetAllAsync(modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(StoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateStoryDto modelDto)
        {
            var responseResult = await storyService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(GetForEdit), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanView)]
        [HttpGet("{id:min(1)}/for-edit")]
        [ProducesResponseType(typeof(GetUpdateStoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var responseResult = await storyService.GetForEditAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdateStoryDto modelDto)
        {
            var responseResult = await storyService.UpdateAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanDelete)]
        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int id)
        {
            var responseResult = await storyService.DeleteAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpPost("{id:min(1)}/description")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddDescription(int id, UpdateStoryDescriptionDto modelDto)
        {
            var responseResult = await storyService.AddDescriptionAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/description/{descriptionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateDescription(int id, string descriptionId, UpdateStoryDescriptionDto modelDto)
        {
            var responseResult = await storyService.UpdateDescriptionAsync(id, descriptionId, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/description/{descriptionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteDescription(int id, string descriptionId)
        {
            var responseResult = await storyService.DeleteDescriptionAsync(id, descriptionId, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanChangeWorker)]
        [HttpPut("{id:min(1)}/storyworker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateStoryWorker(int id, UpdateStoryWorkerDto modelDto)
        {
            var responseResult = await storyService.UpdateStoryWorkerAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/main-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateMainImage(int id, UpdateStoryMainImageDto modelDto)
        {
            var responseResult = await storyService.UpdateMainImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/story-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateStoryImage(int id, UpdateStoryImageDto modelDto)
        {
            var responseResult = await storyService.UpdateStoryImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/story-image/{imageName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteStoryImage(int id, string imageName)
        {
            var responseResult = await storyService.DeleteStoryImageAsync(id, imageName, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanPublish)]
        [HttpPut("{id:min(1)}/publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Publish(int id, PublishStoryDto modelDto)
        {
            var responseResult = await storyService.PublishStoryAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.DotInSiteUser}")]
        [HttpPost("story-list-for-view")]
        [ProducesResponseType(typeof(IEnumerable<GetStoryForListViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForListView(StoryDataTableForViewRequestDto modelDto)
        {
            return Ok(await storyService.GetForListViewAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.DotInSiteUser}")]
        [HttpGet("story-for-view/{subCategoryName}/{storyLink}")]
        [ProducesResponseType(typeof(GetStoryForViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForView(string subCategoryName, string storyLink)
        {
            var responseResult = await storyService.GetForViewAsync(subCategoryName, storyLink).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.DotInSiteUser}")]
        [HttpPost("story-for-view-count/{storyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateStoryForViewCount(int storyId)
        {
            _ = storyViewCounter.IncrementAsync(storyId).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(Roles = $"{RoleConstant.DotInSiteUser}")]
        [HttpGet("sitemap")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSitemap()
        {
            return Ok(await storyService.GetSitemapAsync().ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/story-relative")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateStoryRelative(int id, UpdateStoryRelativeDto modelDto)
        {
            var responseResult = await storyService.UpdateStoryRelativeAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}")]
        [Authorize(Policy = StoryPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/story-relative/{hrefLang}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteStoryRelative(int id, string hrefLang)
        {
            var responseResult = await storyService.DeleteStoryRelativeAsync(id, hrefLang, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }
    }
}
