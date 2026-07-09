using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Dto.DataTableDtos.PostOfficeDataTableDtos;
using Ts.Dto.PostOfficeDtos;
using Ts.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class PostOfficeController : ControllerBase
    {
        private readonly IPostOfficeService postOfficeService;

        public PostOfficeController(IPostOfficeService postOfficeService)
        {
            this.postOfficeService = postOfficeService;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = PostOfficePolicy.CanView)]
        [HttpPost("datatable")]
        [ProducesResponseType(typeof(DataTableResponseDto<GetPostOfficeDataTableDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(PostOfficeDataTableRequestDto modelDto)
        {
            return Ok(await postOfficeService.GetAllAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = PostOfficePolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(PostOfficeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreatePostOfficeDto modelDto)
        {
            var responseResult = await postOfficeService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(Get), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(typeof(PostOfficeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Get(int id)
        {
            var responseResult = await postOfficeService.GetAsync(id).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = PostOfficePolicy.CanView)]
        [HttpGet("{id:min(1)}/for-edit")]
        [ProducesResponseType(typeof(GetUpdatePostOfficeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var responseResult = await postOfficeService.GetForEditAsync(id).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = PostOfficePolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdatePostOfficeDto modelDto)
        {
            var responseResult = await postOfficeService.UpdateAsync(id, modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = PostOfficePolicy.CanDelete)]
        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int id)
        {
            var responseResult = await postOfficeService.DeleteAsync(id, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpGet("by-districtId/{districtId:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<PostOfficeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByDistrictId(int districtId)
        {
            var responseResult = await postOfficeService.GetAllByDistrictIdAsync(districtId).ConfigureAwait(false);
            return Ok(responseResult);
        }
    }
}
