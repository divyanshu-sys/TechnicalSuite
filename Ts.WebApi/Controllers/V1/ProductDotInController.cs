using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
using Ts.ShopIn.Dto.ProductDtos;
using Ts.ShopIn.Dto.ProductImageDtos;
using Ts.ShopIn.Service.DataInterfaces;

namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class ProductDotInController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductDotInController(IProductService productService)
        {
            this.productService = productService;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpPost("datatable/{isShowAll}")]
        [ProducesResponseType(typeof(DataTableResponseDto<GetProductDataTableDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(ProductDataTableRequestDto modelDto, bool isShowAll)
        {
            if (isShowAll)
                return Ok(await productService.GetAllAsync(modelDto).ConfigureAwait(false));
            return Ok(await productService.GetAllAsync(modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateProductDto modelDto)
        {
            var responseResult = await productService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(GetForEdit), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpGet("{id:min(1)}/for-edit")]
        [ProducesResponseType(typeof(GetUpdateProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var responseResult = await productService.GetForEditAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdateProductDto modelDto)
        {
            var responseResult = await productService.UpdateAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanDelete)]
        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int id)
        {
            var responseResult = await productService.DeleteAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanChangeWorker)]
        [HttpPut("{id:min(1)}/productworker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateProductWorker(int id, UpdateProductWorkerDto modelDto)
        {
            var responseResult = await productService.UpdateProductWorkerAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/main-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateMainImage(int id, UpdateProductMainImageDto modelDto)
        {
            var responseResult = await productService.UpdateMainImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/product-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateProductImage(int id, UpdateProductImageDto modelDto)
        {
            var responseResult = await productService.UpdateProductImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/product-image/{imageName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteProductImage(int id, string imageName)
        {
            var responseResult = await productService.DeleteProductImageAsync(id, imageName, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanPublish)]
        [HttpPut("{id:min(1)}/publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Publish(int id, PublishProductDto modelDto)
        {
            var responseResult = await productService.PublishProductAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpPost("product-list-for-view")]
        [ProducesResponseType(typeof(IEnumerable<GetProductForListViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForListView(ProductDataTableForViewRequestDto modelDto)
        {
            return Ok(await productService.GetForListViewAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [HttpGet("dropdown")]
        [ProducesResponseType(typeof(IEnumerable<DropdownItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllForDropDownAsync()
        {
            return Ok(await productService.GetAllForDropDownAsync().ConfigureAwait(false));
        }
    }
}
