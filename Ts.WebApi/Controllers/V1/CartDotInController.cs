using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.ShopIn.Dto.CartDtos;
using Ts.ShopIn.Service.DataInterfaces;

namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class CartDotInController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartDotInController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(CreateCartDto modelDto)
        {
            var responseResult = await cartService.CreateAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return CreatedAtAction(nameof(GetByUserId), responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpGet("getbyuser")]
        [ProducesResponseType(typeof(GetCartForListViewDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId()
        {
            var responseResult = await cartService.GetByUserIdAsync(User.Claims.GetUserId()).ConfigureAwait(false);
            return Ok(responseResult);
        }

        [HttpDelete("{productDetailId:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete(int productDetailId)
        {
            var responseResult = await cartService.DeleteAsync(productDetailId, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }
    }
}
