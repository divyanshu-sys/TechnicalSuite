using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Dto.CategoryDtos;
using Ts.Dto.DataTableDtos.CategoryDataTableDtos;
using Ts.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = CategoryPolicy.CanView)]
        [HttpPost("datatable")]
        [ProducesResponseType(typeof(DataTableResponseDto<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(CategoryDataTableRequestDto modelDto)
        {
            return Ok(await categoryService.GetAllAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = CategoryPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateCategoryDto modelDto)
        {
            var responseResult = await categoryService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(Get), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = CategoryPolicy.CanView)]
        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Get(int id)
        {
            var responseResult = await categoryService.GetAsync(id).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = CategoryPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdateCategoryDto modelDto)
        {
            var responseResult = await categoryService.UpdateAsync(id, modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await categoryService.GetAllAsync().ConfigureAwait(false));
        }
    }
}
