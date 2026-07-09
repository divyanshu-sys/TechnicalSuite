using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DataTableDtos.ApplicationUserDataTableDtos;
using Ts.Dto.DataTableDtos.ClientUserDataTableDtos;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Dto.ClientUserDtos;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly IClientUserService clientUserService;

        public ApplicationUserController(IApplicationUserService applicationUserService, IClientUserService clientUserService)
        {
            this.applicationUserService = applicationUserService;
            this.clientUserService = clientUserService;
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [Authorize(Policy = UserPolicy.CanView)]
        [HttpPost("shopin-datatable")]
        [ProducesResponseType(typeof(DataTableResponseDto<ClientUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ShopInDataTable([FromBody] ClientUserDataTableRequestDto modelDto)
        {
            return Ok(await clientUserService.GetAllAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [Authorize(Policy = UserPolicy.CanView)]
        [HttpPost("datatable")]
        [ProducesResponseType(typeof(DataTableResponseDto<ApplicationUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DataTable([FromBody] ApplicationUserDataTableRequestDto modelDto)
        {
            return Ok(await applicationUserService.GetAllAsync(modelDto).ConfigureAwait(false));
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [Authorize(Policy = UserPolicy.CanView)]
        [HttpGet("{userId}/for-edit")]
        [ProducesResponseType(typeof(GetUpdateApplicationUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetForEdit(string userId)
        {
            var responseResult = await applicationUserService.GetForEditAsync(userId).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}")]
        [Authorize(Policy = UserPolicy.CanUpdate)]
        [HttpPut("{userId}/for-edit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put(string userId, UpdateApplicationUserDto modelDto)
        {
            var responseResult = await applicationUserService.UpdateForEditAsync(userId, modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpGet("profile")]
        [ProducesResponseType(typeof(GetProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserProfile()
        {
            var responseResult = await applicationUserService.GetUserProfileAsync(User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [HttpPost("change-email")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto modelDto)
        {
            var responseResult = await applicationUserService.ChangeEmailAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
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
            var responseResult = await applicationUserService.ConfirmChangeEmailAsync(modelDto).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [Authorize(Policy = UserPolicy.CanUpdateUserEmail)]
        [HttpPost("{userId}/update-email")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUserEmail(string userId, [FromBody] ChangeEmailDto modelDto)
        {
            var responseResult = await applicationUserService.ChangeEmailAsync(modelDto, userId).ConfigureAwait(false);
            if (responseResult.Data)
                return Accepted();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [Authorize(Policy = UserPolicy.CanUpdateUserPrivilege)]
        [HttpGet("{userId}/user-privilege")]
        [ProducesResponseType(typeof(GetApplicationUserPrivilegeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserPrivilege(string userId)
        {
            var responseResult = await applicationUserService.GetUserPrivilegeAsync(userId).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [Authorize(Policy = UserPolicy.CanUpdateUserPrivilege)]
        [HttpPut("{userId}/user-privilege")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUserPrivilege(string userId, [FromBody] UpdateApplicationUserPrivilegeDto modelDto)
        {
            var responseResult = await applicationUserService.UpdateUserPrivilegeAsync(modelDto, userId, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.Employee}, {RoleConstant.EmployeeShopIn}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await applicationUserService.GetAllByRolesAsync(
                [RoleConstant.Administrator, RoleConstant.Employee, RoleConstant.EmployeeShopIn], User.Claims.GetUserId(),
                User.IsInRole(RoleConstant.Administrator)
                ).ConfigureAwait(false));
        }
    }
}
