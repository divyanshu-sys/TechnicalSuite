using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.ClientUserVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface IApplicationUserClient
    {
        Task<ResponseMessageDto<AuthTokenDto>> LoginAsync(LoginVm model);
        Task<ResponseMessageDto<bool>> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<ResponseMessageDto<bool>> ConfirmEmailAsync(ConfirmEmailDto model);
        Task<ResponseMessageDto<bool>> ResetPasswordAsync(ResetPasswordVm model, string encUserId, string token);
        Task<ResponseMessageDto<bool>> ChangePasswordAsync(ChangePasswordVm model);
        Task<ResponseMessageDto<bool>> IsEmailAvailableAsync(string email);
        Task<ResponseMessageDto<DataTableResponseVm<ApplicationUserVm>>> GetAllAsync(ApplicationUserDataTableRequestVm model);
        Task<ResponseMessageDto<DataTableResponseVm<ClientUserVm>>> GetAllShopInUserAsync(ClientUserDataTableRequestVm model);
        Task<ResponseMessageDto<bool>> RegisterUserAsync(RegisterApplicationUserVm model);
        Task<ResponseMessageDto<UpdateApplicationUserVm>> GetForEditAsync(string userId);
        Task<ResponseMessageDto<bool>> UpdateForEditAsync(UpdateApplicationUserVm model, string userId);
        Task<ResponseMessageDto<ApplicationUserVm>> GetUserProfileAsync();
        Task<ResponseMessageDto<bool>> ChangeEmailAsync(ChangeEmailVm model);
        Task<ResponseMessageDto<bool>> ConfirmChangeEmailAsync(ConfirmChangeEmailDto model);
        Task<ResponseMessageDto<bool>> UpdateUserEmailAsync(ChangeEmailVm model, string userId);
        Task<ResponseMessageDto<UpdatePrivilegeVm>> GetUserPrivilegeAsync(string userId);
        Task<ResponseMessageDto<bool>> UpdateUserPrivilegeAsync(UpdatePrivilegeVm model, string userId);
        Task<ResponseMessageDto<IEnumerable<ApplicationUserVm>>> GetAllAsync();
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<string, string>>>> GetAllForDropDownAsync();
    }
}
