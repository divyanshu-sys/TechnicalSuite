using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Dto.DataTableDtos.DistrictDataTableDtos;
using Ts.Dto.DistrictDtos;
using Ts.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService districtService;

        public DistrictController(IDistrictService districtService)
        {
            this.districtService = districtService;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = DistrictPolicy.CanView)]
        [HttpPost("datatable")]
        [ProducesResponseType(typeof(DataTableResponseDto<GetDistrictDataTableDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(DistrictDataTableRequestDto modelDto)
        {
            return Ok(await districtService.GetAllAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = DistrictPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(DistrictDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateDistrictDto modelDto)
        {
            var responseResult = await districtService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(Get), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(typeof(DistrictDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Get(int id)
        {
            var responseResult = await districtService.GetAsync(id).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = DistrictPolicy.CanView)]
        [HttpGet("{id:min(1)}/for-edit")]
        [ProducesResponseType(typeof(GetUpdateDistrictDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var responseResult = await districtService.GetForEditAsync(id).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = DistrictPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdateDistrictDto modelDto)
        {
            var responseResult = await districtService.UpdateAsync(id, modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = DistrictPolicy.CanDelete)]
        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int id)
        {
            var responseResult = await districtService.DeleteAsync(id, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpGet("by-stateId/{stateId:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<DistrictDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByCountryId(int stateId)
        {
            var responseResult = await districtService.GetAllByStateIdAsync(stateId).ConfigureAwait(false);
            return Ok(responseResult);
        }
    }
}
