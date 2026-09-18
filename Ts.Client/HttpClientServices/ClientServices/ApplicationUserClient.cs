using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.ClientUserVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.ApplicationUserDataTableDtos;
using Ts.Dto.DataTableDtos.ClientUserDataTableDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class ApplicationUserClient : IApplicationUserClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public ApplicationUserClient(IHttpClientService httpClientService,
            IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<AuthTokenDto>> LoginAsync(LoginVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<LoginDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PostAsync<AuthTokenDto>($"{BaseUrl}/{ApiUrl}/authapplicationuser", modelDto).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/authapplicationuser/forgot-password", model).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ConfirmEmailAsync(ConfirmEmailDto model)
        {
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/authapplicationuser/confirm-email", model).ConfigureAwait(false);
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

            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/authapplicationuser/reset-password", modelDto).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ChangePasswordAsync(ChangePasswordVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<ChangePasswordDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/authapplicationuser/change-password", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> IsEmailAvailableAsync(string email)
        {
            var modelDto = new IsEmailAvailableDto
            {
                Email = email.Trim()
            };
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/authapplicationuser/is-email-available", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<ApplicationUserVm>>> GetAllAsync(ApplicationUserDataTableRequestVm model)
        {
            var postModel = mapper.Map<ApplicationUserDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<ApplicationUserOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 1)
                        sort.OrderBy.FirstName = true;
                    else if (order.Column == 2)
                        sort.OrderBy.LastName = true;
                    else if (order.Column == 4)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 5)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    postModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<ApplicationUserVm>>($"{BaseUrl}/{ApiUrl}/applicationuser/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<ClientUserVm>>> GetAllShopInUserAsync(ClientUserDataTableRequestVm model)
        {
            var postModel = mapper.Map<ClientUserDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<ClientUserOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 1)
                        sort.OrderBy.FirstName = true;
                    else if (order.Column == 2)
                        sort.OrderBy.LastName = true;
                    else if (order.Column == 5)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 6)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    postModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<ClientUserVm>>($"{BaseUrl}/{ApiUrl}/applicationuser/shopin-datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<bool>> RegisterUserAsync(RegisterApplicationUserVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<RegisterApplicationUserDto>(model);
            modelDto.IpAddress = ipAddress;
            modelDto.Address.IpAddress = ipAddress;

            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/authapplicationuser/register-user", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<ApplicationUserVm>> GetUserProfileAsync()
        {
            return await httpClientService.GetAsync<ApplicationUserVm>($"{BaseUrl}/{ApiUrl}/applicationuser/profile", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateApplicationUserVm>> GetForEditAsync(string userId)
        {
            var vm = await httpClientService.GetAsync<ApplicationUserVm>($"{BaseUrl}/{ApiUrl}/applicationuser/{userId}/for-edit", true).ConfigureAwait(false);

            var updateVm = mapper.Map<ResponseMessageDto<UpdateApplicationUserVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<bool>> UpdateForEditAsync(UpdateApplicationUserVm model, string userId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateApplicationUserDto>(model);
            modelDto.IpAddress = ipAddress;
            modelDto.Address.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/applicationuser/{userId}/for-edit", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ChangeEmailAsync(ChangeEmailVm model)
        {
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/applicationuser/change-email", model, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> ConfirmChangeEmailAsync(ConfirmChangeEmailDto model)
        {
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/applicationuser/confirm-change-email", model).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateUserEmailAsync(ChangeEmailVm model, string userId)
        {
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/applicationuser/{userId}/update-email", model, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdatePrivilegeVm>> GetUserPrivilegeAsync(string userId)
        {
            return await httpClientService.GetAsync<UpdatePrivilegeVm>($"{BaseUrl}/{ApiUrl}/applicationuser/{userId}/user-privilege", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> UpdateUserPrivilegeAsync(UpdatePrivilegeVm model, string userId)
        {
            var modelDto = mapper.Map<UpdateApplicationUserPrivilegeDto>(model);
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/applicationuser/{userId}/user-privilege", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<ApplicationUserVm>>> GetAllAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<ApplicationUserVm>>($"{BaseUrl}/{ApiUrl}/applicationuser", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<string, string>>>> GetAllForDropDownAsync()
        {
            var vms = await GetAllAsync().ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<string, string>>>>(vms);
            return dropDown;
        }
    }
}
