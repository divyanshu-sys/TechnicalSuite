using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DataTableDtos.ClientUserDataTableDtos;
using Ts.Dto.RefreshTokenDtos;
using Ts.ShopIn.Dto.ClientUserDtos;
namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IClientUserService
    {
        Task<ResponseMessageDto<AuthTokenDto>> GetAuthenticationTokenAsync(LoginDto modelDto);

        Task<ResponseMessageDto<ApiTokenDto>> GetRefreshApiTokenAsync(AuthRefreshDto modelDto);

        Task<ResponseMessageDto<bool>> ForgotPasswordAsync(ForgotPasswordDto modelDto);

        Task<ResponseMessageDto<bool>> ResetPasswordAsync(ResetPasswordDto modelDto);

        Task<ResponseMessageDto<bool>> ConfirmEmailAsync(ConfirmEmailDto modelDto);

        Task<ResponseMessageDto<bool>> ChangePasswordAsync(ChangePasswordDto modelDto, string userId);

        Task<bool> IsEmailAvailableAsync(string email);

        Task<DataTableResponseDto<ClientUserDto>> GetAllAsync(ClientUserDataTableRequestDto modelDto);

        Task<ResponseMessageDto<ClientUserDto>> RegisterUserAsync(RegisterClientUserDto modelDto);

        Task<ResponseMessageDto<ClientUserDto>> GetForEditAsync(string userId);

        Task<ResponseMessageDto<bool>> UpdateForEditAsync(string editUserId, UpdateClientUserDto modelDto, string userId);

        Task<ResponseMessageDto<ClientUserDto>> GetUserProfileAsync(string userId);

        Task<ResponseMessageDto<bool>> ChangeEmailAsync(ChangeEmailDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> ConfirmChangeEmailAsync(ConfirmChangeEmailDto modelDto);
    }
}
