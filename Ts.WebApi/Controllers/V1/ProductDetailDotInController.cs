using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;
using Ts.ShopIn.Dto.ProductDetailImageDtos;
using Ts.ShopIn.Service.DataInterfaces;

namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class ProductDetailDotInController : ControllerBase
    {
        private readonly IProductDetailService productDetailService;
        private readonly IProductDetailViewCounter productDetailViewCounter;

        public ProductDetailDotInController(IProductDetailService productDetailService,
            IProductDetailViewCounter productDetailViewCounter)
        {
            this.productDetailService = productDetailService;
            this.productDetailViewCounter = productDetailViewCounter;
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpPost("datatable/{isShowAll}")]
        [ProducesResponseType(typeof(DataTableResponseDto<GetProductDetailDataTableDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable(ProductDetailDataTableRequestDto modelDto, bool isShowAll)
        {
            if (isShowAll)
                return Ok(await productDetailService.GetAllAsync(modelDto).ConfigureAwait(false));
            return Ok(await productDetailService.GetAllAsync(modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanCreate)]
        [HttpPost]
        [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateProductDetailDto modelDto)
        {
            var responseResult = await productDetailService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(GetForEdit), new { id = responseResult.Data.Id }, responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpGet("{id:min(1)}/for-edit")]
        [ProducesResponseType(typeof(GetUpdateProductDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var responseResult = await productDetailService.GetForEditAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(int id, UpdateProductDetailDto modelDto)
        {
            var responseResult = await productDetailService.UpdateAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
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
            var responseResult = await productDetailService.DeleteAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPatch("{id:min(1)}/description")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PatchDescription(int id, UpdateProductDetailDescriptionDto modelDto)
        {
            var responseResult = await productDetailService.UpdateDescriptionAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanChangeWorker)]
        [HttpPut("{id:min(1)}/productdetailworker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateProductDetailWorker(int id, UpdateProductDetailWorkerDto modelDto)
        {
            var responseResult = await productDetailService.UpdateProductDetailWorkerAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/main-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateMainImage(int id, UpdateProductDetailMainImageDto modelDto)
        {
            var responseResult = await productDetailService.UpdateMainImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/productdetail-image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateProductDetailImage(int id, UpdateProductDetailImageDto modelDto)
        {
            var responseResult = await productDetailService.UpdateProductDetailImageAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/productdetail-image/{imageName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteProductDetailImage(int id, string imageName)
        {
            var responseResult = await productDetailService.DeleteProductDetailImageAsync(id, imageName, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpPut("{id:min(1)}/productdetail-document")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateProductDetailDocument(int id, UpdateProductDetailDocumentDto modelDto)
        {
            var responseResult = await productDetailService.UpdateProductDetailDocumentAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanUpdate)]
        [HttpDelete("{id:min(1)}/productdetail-document")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteProductDetailDocument(int id)
        {
            var responseResult = await productDetailService.DeleteProductDetailDocumentAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanPublish)]
        [HttpPut("{id:min(1)}/publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Publish(int id, PublishProductDetailDto modelDto)
        {
            var responseResult = await productDetailService.PublishProductDetailAsync(id, modelDto, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpPost("productdetail-list-for-view")]
        [ProducesResponseType(typeof(IEnumerable<GetProductDetailForListViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForListView(ProductDetailDataTableForViewRequestDto modelDto)
        {
            return Ok(await productDetailService.GetForListViewAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpGet("productdetail-for-view/{shopCategoryName}/{productdetailLink}")]
        [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForView(string shopCategoryName, string productdetailLink)
        {
            var responseResult = await productDetailService.GetForViewAsync(shopCategoryName, productdetailLink).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpPost("productdetail-for-view-count/{productdetailId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateProductDetailForViewCount(int productdetailId)
        {
            _ = productDetailViewCounter.IncrementAsync(productdetailId).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpGet("sitemap")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSitemap()
        {
            return Ok(await productDetailService.GetSitemapAsync().ConfigureAwait(false));
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpGet("total-unique-pages-visited")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalUniquePagesVisited(DateTimeOffset? lastViewedOnStart, DateTimeOffset? lastViewedOnEnd)
        {
            var response = await productDetailService.GetTotalUniquePagesVisitedAsync(lastViewedOnStart, lastViewedOnEnd).ConfigureAwait(false);
            return Ok(response);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpGet("total-pages-visited-lifetime")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPagesVisitedLifetime()
        {
            var response = await productDetailService.GetTotalPagesVisitedLifetimeAsync().ConfigureAwait(false);
            return Ok(response);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpGet("{id:min(1)}/productdetail-document")]
        [ProducesResponseType(typeof(DownloadDocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetProductDocumentDetail(int id)
        {
            var responseResult = await productDetailService.GetProductDocumentDetailAsync(id, User.Claims.GetUserId(), User.IsInRole(RoleConstant.Administrator)).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }
    }
}
