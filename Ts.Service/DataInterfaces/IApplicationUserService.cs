using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DataTableDtos.ApplicationUserDataTableDtos;
using Ts.Dto.RefreshTokenDtos;
namespace Ts.Service.DataInterfaces
{
    public interface IApplicationUserService
    {
        Task<ResponseMessageDto<AuthTokenDto>> GetAuthenticationTokenAsync(LoginDto modelDto);

        Task<ResponseMessageDto<ApiTokenDto>> GetRefreshApiTokenAsync(AuthRefreshDto modelDto);

        Task<ResponseMessageDto<bool>> ForgotPasswordAsync(ForgotPasswordDto modelDto);

        Task<ResponseMessageDto<bool>> ResetPasswordAsync(ResetPasswordDto modelDto);

        Task<ResponseMessageDto<bool>> ConfirmEmailAsync(ConfirmEmailDto modelDto);

        Task<ResponseMessageDto<bool>> ChangePasswordAsync(ChangePasswordDto modelDto, string userId);

        Task<bool> IsEmailAvailableAsync(string email);

        Task<DataTableResponseDto<ApplicationUserDto>> GetAllAsync(ApplicationUserDataTableRequestDto modelDto);

        Task<ResponseMessageDto<ApplicationUserDto>> RegisterUserAsync(RegisterApplicationUserDto modelDto, string userId);

        Task<ResponseMessageDto<GetUpdateApplicationUserDto>> GetForEditAsync(string userId);

        Task<ResponseMessageDto<bool>> UpdateForEditAsync(string editUserId, UpdateApplicationUserDto modelDto, string userId);

        Task<ResponseMessageDto<GetProfileDto>> GetUserProfileAsync(string userId);

        Task<ResponseMessageDto<bool>> ChangeEmailAsync(ChangeEmailDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> ConfirmChangeEmailAsync(ConfirmChangeEmailDto modelDto);

        Task<ResponseMessageDto<GetApplicationUserPrivilegeDto>> GetUserPrivilegeAsync(string userId);

        Task<ResponseMessageDto<bool>> UpdateUserPrivilegeAsync(UpdateApplicationUserPrivilegeDto modelDto, string updateUserId, string userId);

        Task<IEnumerable<ApplicationUserDto>> GetAllByRolesAsync(IEnumerable<string> roles, string userId, bool isAdmin);

        Task<IEnumerable<ApplicationUserDto>> GetUsersByUserIdsAsync(IEnumerable<string> userIds);
    }
}
