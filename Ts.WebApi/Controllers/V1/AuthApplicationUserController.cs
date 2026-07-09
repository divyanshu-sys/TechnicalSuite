using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.RefreshTokenDtos;
using Ts.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class AuthApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService applicationUserService;

        public AuthApplicationUserController(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }

        /// <summary>
        /// Authenticates a user and returns an authentication token.
        /// </summary>
        /// <param name="modelDto">The login credentials containing username/email and password.</param>
        /// <returns>An <see cref="AuthTokenDto"/> containing the authentication token on success.</returns>
        /// <response code="200">Authentication successful, returns auth token.</response>
        /// <response code="422">Authentication failed, returns error messages.</response>
        /// <remarks>
        /// Sample value for LoginDto model
        /// 
        ///     {
        ///         "email": "info@domain.com",
        ///         "password": "Test@123"
        ///         "ipAddress": "::1"
        ///     }
        ///     
        /// </remarks>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(AuthTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Login([FromBody] LoginDto modelDto)
        {
            var responseResult = await applicationUserService.GetAuthenticationTokenAsync(modelDto).ConfigureAwait(false);
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
            var responseResult = await applicationUserService.ForgotPasswordAsync(modelDto).ConfigureAwait(false);
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
            var responseResult = await applicationUserService.ResetPasswordAsync(modelDto).ConfigureAwait(false);
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
            var responseResult = await applicationUserService.ConfirmEmailAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto modelDto)
        {
            var responseResult = await applicationUserService.ChangePasswordAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
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
            var responseResult = await applicationUserService.GetRefreshApiTokenAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpPost("is-email-available")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsEmailAvailable(IsEmailAvailableDto modelDto)
        {
            return Ok(await applicationUserService.IsEmailAvailableAsync(modelDto.Email).ConfigureAwait(false));
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [Authorize(Policy = UserPolicy.CanCreate)]
        [HttpPost("register-user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterApplicationUserDto modelDto)
        {
            var responseResult = await applicationUserService.RegisterUserAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }
    }
}
