using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Dto.DataTableDtos.SubCategoryDataTableDtos;
using Ts.Dto.SubCategoryDtos;
using Ts.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            this.subCategoryService = subCategoryService;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = SubCategoryPolicy.CanView)]
        [HttpPost("datatable")]
        [ProducesResponseType(typeof(DataTableResponseDto<SubCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(SubCategoryDataTableRequestDto modelDto)
        {
            return Ok(await subCategoryService.GetAllAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = SubCategoryPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(SubCategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateSubCategoryDto modelDto)
        {
            var responseResult = await subCategoryService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(Get), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = SubCategoryPolicy.CanView)]
        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(typeof(SubCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Get(int id)
        {
            var responseResult = await subCategoryService.GetAsync(id).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = SubCategoryPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdateSubCategoryDto modelDto)
        {
            var responseResult = await subCategoryService.UpdateAsync(id, modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await subCategoryService.GetAllAsync().ConfigureAwait(false));
        }
    }
}
