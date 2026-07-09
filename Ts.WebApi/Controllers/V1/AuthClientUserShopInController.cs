using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.RefreshTokenDtos;
using Ts.ShopIn.Dto.ClientUserDtos;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class AuthClientUserShopInController : ControllerBase
    {
        private readonly IClientUserService clientUserService;

        public AuthClientUserShopInController(IClientUserService clientUserService)
        {
            this.clientUserService = clientUserService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(AuthTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Login([FromBody] LoginDto modelDto)
        {
            var responseResult = await clientUserService.GetAuthenticationTokenAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto modelDto)
        {
            var responseResult = await clientUserService.ForgotPasswordAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data)
                return Accepted();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPut("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto modelDto)
        {
            var responseResult = await clientUserService.ResetPasswordAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPut("confirm-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto modelDto)
        {
            var responseResult = await clientUserService.ConfirmEmailAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto modelDto)
        {
            var responseResult = await clientUserService.ChangePasswordAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RefreshApiToken([FromBody] AuthRefreshDto modelDto)
        {
            var responseResult = await clientUserService.GetRefreshApiTokenAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost("is-email-available")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsEmailAvailable(IsEmailAvailableDto modelDto)
        {
            return Ok(await clientUserService.IsEmailAvailableAsync(modelDto.Email).ConfigureAwait(false));
        }

        [AllowAnonymous]
        [HttpPost("register-user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterClientUserDto modelDto)
        {
            var responseResult = await clientUserService.RegisterUserAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data != null)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }
    }
}
