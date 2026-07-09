using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.ClientUserVms;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ClientUserVms;
using Ts.ShopIn.Dto.ClientUserDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class ClientUserClient : IClientUserClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientUserService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public ClientUserClient(IHttpClientUserService httpClientService,
            IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<AuthTokenDto>> LoginAsync(LoginVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<LoginDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PostAsync<AuthTokenDto>($"{BaseUrl}/{ApiUrl}/authclientusershopin", modelDto).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/authclientusershopin/forgot-password", model).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ConfirmEmailAsync(ConfirmEmailDto model)
        {
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/authclientusershopin/confirm-email", model).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ResetPasswordAsync(ResetPasswordVm model, string encUserId, string token)
        {
            if (encUserId == null)
                throw new ArgumentNullException(ExceptionMessageConstant.ValueNotSpecified, nameof(encUserId));

            if (token == null)
                throw new ArgumentNullException(ExceptionMessageConstant.ValueNotSpecified, nameof(token));

            var modelDto = mapper.Map<ResetPasswordDto>(model);
            modelDto.EncUserId = encUserId;
            modelDto.Token = token;

            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/authclientusershopin/reset-password", modelDto).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ChangePasswordAsync(ChangePasswordVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<ChangePasswordDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/authclientusershopin/change-password", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> IsEmailAvailableAsync(string email)
        {
            var modelDto = new IsEmailAvailableDto
            {
                Email = email.Trim()
            };
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/authclientusershopin/is-email-available", modelDto).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> RegisterUserAsync(RegisterClientUserVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<RegisterClientUserDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/authclientusershopin/register-user", modelDto).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<ClientUserVm>> GetUserProfileAsync()
        {
            return await httpClientService.GetAsync<ClientUserVm>($"{BaseUrl}/{ApiUrl}/clientusershopin/profile", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateClientUserVm>> GetForEditAsync()
        {
            var vm = await httpClientService.GetAsync<ClientUserVm>($"{BaseUrl}/{ApiUrl}/clientusershopin/for-edit", true).ConfigureAwait(false);

            var updateVm = mapper.Map<ResponseMessageDto<UpdateClientUserVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<bool>> UpdateForEditAsync(UpdateClientUserVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateClientUserDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/clientusershopin/for-edit", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ChangeEmailAsync(ChangeEmailVm model)
        {
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/clientusershopin/change-email", model, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ConfirmChangeEmailAsync(ConfirmChangeEmailDto model)
        {
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/clientusershopin/confirm-change-email", model).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateUserEmailAsync(ChangeEmailVm model)
        {
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/clientusershopin/update-email", model, true).ConfigureAwait(false);
        }
    }
}
