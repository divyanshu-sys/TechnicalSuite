using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto.ApplicationUserDtos;
using Ts.ShopIn.Dto.ClientUserDtos;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class ClientUsershopInController : ControllerBase
    {
        private readonly IClientUserService clientUserService;

        public ClientUsershopInController(IClientUserService clientUserService)
        {
            this.clientUserService = clientUserService;
        }

        [HttpGet("for-edit")]
        [ProducesResponseType(typeof(ClientUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit()
        {
            var responseResult = await clientUserService.GetForEditAsync(User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpPut("for-edit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(UpdateClientUserDto modelDto)
        {
            var responseResult = await clientUserService.UpdateForEditAsync(User.Claims.GetUserId(), modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpGet("profile")]
        [ProducesResponseType(typeof(ClientUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserProfile()
        {
            var responseResult = await clientUserService.GetUserProfileAsync(User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpPost("change-email")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto modelDto)
        {
            var responseResult = await clientUserService.ChangeEmailAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return Accepted();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPut("confirm-change-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ConfirmChangeEmail([FromBody] ConfirmChangeEmailDto modelDto)
        {
            var responseResult = await clientUserService.ConfirmChangeEmailAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }
    }
}
