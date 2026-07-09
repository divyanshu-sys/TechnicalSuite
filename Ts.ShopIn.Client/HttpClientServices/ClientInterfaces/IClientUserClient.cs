using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.ClientUserVms;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.ShopIn.Client.ViewModels.ClientUserVms;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IClientUserClient
    {
        Task<ResponseMessageDto<AuthTokenDto>> LoginAsync(LoginVm model);
        Task<ResponseMessageDto<bool>> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<ResponseMessageDto<bool>> ConfirmEmailAsync(ConfirmEmailDto model);
        Task<ResponseMessageDto<bool>> ResetPasswordAsync(ResetPasswordVm model, string encUserId, string token);
        Task<ResponseMessageDto<bool>> ChangePasswordAsync(ChangePasswordVm model);
        Task<ResponseMessageDto<bool>> IsEmailAvailableAsync(string email);
        Task<ResponseMessageDto<bool>> RegisterUserAsync(RegisterClientUserVm model);
        Task<ResponseMessageDto<UpdateClientUserVm>> GetForEditAsync();
        Task<ResponseMessageDto<bool>> UpdateForEditAsync(UpdateClientUserVm model);
        Task<ResponseMessageDto<ClientUserVm>> GetUserProfileAsync();
        Task<ResponseMessageDto<bool>> ChangeEmailAsync(ChangeEmailVm model);
        Task<ResponseMessageDto<bool>> ConfirmChangeEmailAsync(ConfirmChangeEmailDto model);
        Task<ResponseMessageDto<bool>> UpdateUserEmailAsync(ChangeEmailVm model);
    }
}
