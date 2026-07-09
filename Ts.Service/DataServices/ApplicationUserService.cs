using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Ts.Application.AppConstants;
using Ts.Application.AppSettings;
using Ts.Application.Helpers;
using Ts.Common.AppInterfaces;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.DataTableModels.ApplicationUserDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto;
using Ts.Dto.AddressDtos;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DataTableDtos.ApplicationUserDataTableDtos;
using Ts.Dto.RefreshTokenDtos;
using Ts.Service.DataInterfaces;
namespace Ts.Service.DataServices
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailMessageService emailMessageService;
        private readonly IConfiguration config;
        private readonly JwtSetting jwtSetting;
        private readonly ITimeLimitedDataProtector confirmMailProtector;
        private readonly ITimeLimitedDataProtector changeEmailProtector;
        private readonly ITimeLimitedDataProtector resetPasswordProtector;
        private readonly ILogger<ApplicationUserService> logger;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;
        private readonly IUnitOfWork unitOfWork;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IRandomService randomService;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, IEmailMessageService emailMessageService,
            IOptions<JwtSetting> jwtSettings, IConfiguration config,
            IDataProtectionProvider dataProtectorProvider, ILogger<ApplicationUserService> logger,
            IMapper mapper, IDistributedCache cache,
            IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager, IRandomService randomService)
        {
            this.userManager = userManager;
            this.emailMessageService = emailMessageService;
            this.jwtSetting = jwtSettings.Value;
            this.config = config;
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            this.randomService = randomService;
            confirmMailProtector = dataProtectorProvider.CreateProtector(DataProtectionPurposeStringConstant.EmailConfirmationProtectionKey).ToTimeLimitedDataProtector();
            changeEmailProtector = dataProtectorProvider.CreateProtector(DataProtectionPurposeStringConstant.ChangeEmailProtectionKey).ToTimeLimitedDataProtector();
            resetPasswordProtector = dataProtectorProvider.CreateProtector(DataProtectionPurposeStringConstant.ResetPasswordProtectionKey).ToTimeLimitedDataProtector();
        }

        public async Task<ResponseMessageDto<ApiTokenDto>> GetRefreshApiTokenAsync(AuthRefreshDto modelDto)
        {
            var responseResult = new ResponseMessageDto<ApiTokenDto>();

            AuthRefreshEncodeDto authRefreshEncodeDto;
            try
            {
                var decodedRefreshToken = AesEndeCryptor.DecryptString(EnDecryptionConstant.RefreshTokenKey, EnDecryptionConstant.RefreshTokenIv, modelDto.EncodedRefreshToken);
                authRefreshEncodeDto = JsonConvert.DeserializeObject<AuthRefreshEncodeDto>(decodedRefreshToken);
            }
            catch
            {
                responseResult.ErrorMessage.Add("Invalid refresh token supplied.");
                return responseResult;
            }

            if (authRefreshEncodeDto.ExpireAt < DateTime.UtcNow)
            {
                responseResult.ErrorMessage.Add("Refresh token has expired.");
                return responseResult;
            }

            var refreshToken = await unitOfWork.RefreshTokenRepo.GetAsync(authRefreshEncodeDto.RefreshId).ConfigureAwait(false);
            if (refreshToken == null)
            {
                responseResult.ErrorMessage.Add("Refresh token not found.");
                return responseResult;
            }

            var user = await userManager.FindByIdAsync(refreshToken.UserId).ConfigureAwait(false);
            if (user != null && user.CanLogin && user.EmailConfirmed &&
                (!user.LockoutEnabled || !await userManager.IsLockedOutAsync(user).ConfigureAwait(false)))
            {
                var apiToken = await GetApiTokenAsync(user, DateTime.UtcNow, refreshToken.RefreshReloginId, authRefreshEncodeDto.ExpireAt.Value).ConfigureAwait(false);
                responseResult.Data = apiToken;
                return responseResult;
            }

            responseResult.ErrorMessage.Add("Failed to get refreshed api token.");

            return responseResult;
        }

        public async Task<ResponseMessageDto<AuthTokenDto>> GetAuthenticationTokenAsync(LoginDto modelDto)
        {
            var responseResult = new ResponseMessageDto<AuthTokenDto>();

            ApplicationUser user;
            if (Regex.IsMatch(modelDto.Email, RegxConstant.Email))
                user = await userManager.FindByEmailAsync(modelDto.Email).ConfigureAwait(false);
            else
                user = await userManager.FindByNameAsync(modelDto.Email).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("Invalid login user.");
                return responseResult;
            }

            if (!user.CanLogin)
            {
                responseResult.ErrorMessage.Add("You cannot login! Please contact administrator.");
                return responseResult;
            }

            if (user.LockoutEnabled && await userManager.IsLockedOutAsync(user).ConfigureAwait(false))
            {
                responseResult.ErrorMessage.Add("Your account is lockedout! Please contact administrator.");
                return responseResult;
            }

            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, modelDto.Password).ConfigureAwait(false);

            if (isPasswordCorrect && !user.EmailConfirmed)
            {
                bool success = await SendEmailConfirmationAsync(user).ConfigureAwait(false);
                responseResult.ErrorMessage.Add("Email not confirmed yet! Please check your email account and gets email confirmed.");
                if (!success)
                    responseResult.ErrorMessage.Add("Problem in sending email verification link.");
                return responseResult;
            }

            if (!isPasswordCorrect)
            {
                if (!user.LockoutEnabled)
                {
                    responseResult.ErrorMessage.Add("Invalid login attempt.");
                    return responseResult;
                }

                int maxLoginFailedAccessAttempts = int.Parse(config["MaxLoginFailedAccessAttempts"]);

                var accessResult = await userManager.AccessFailedAsync(user).ConfigureAwait(false);
                if (!accessResult.Succeeded && accessResult.Errors.Any())
                {
                    foreach (var error in accessResult.Errors)
                        responseResult.ErrorMessage.Add(error.Description);
                    return responseResult;
                }

                var accessFailedCount = await userManager.GetAccessFailedCountAsync(user).ConfigureAwait(false);

                if (await userManager.IsLockedOutAsync(user).ConfigureAwait(false))
                    responseResult.ErrorMessage.Add("Your account is lockedout!");
                else
                    responseResult.ErrorMessage.Add($"Invalid login. {maxLoginFailedAccessAttempts - accessFailedCount} attempts left.");
            }
            else
            {
                var refreshToken = await unitOfWork.RefreshTokenRepo.GetByUserIdAsync(user.Id).ConfigureAwait(false);
                try
                {
                    refreshToken ??= RefreshTokenCommonService.CreateByUserId(user.Id, unitOfWork);
                    await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateException ex)
                {
                    refreshToken = await unitOfWork.RefreshTokenRepo.GetByUserIdAsync(user.Id).ConfigureAwait(false);
                    if (refreshToken == null)
                    {
                        await Task.Delay(50);
                        refreshToken = await unitOfWork.RefreshTokenRepo.GetByUserIdAsync(user.Id).ConfigureAwait(false);
                    }

                    logger.LogError(ex, "An error occurred while updating the refresh token for user {UserId}.", user.Id);
                }

                DateTime utcDateTime = DateTime.UtcNow;
                var refreshTokenExpiryAt = utcDateTime.Add(TimeSpan.FromMinutes(jwtSetting.RefreshTokenExpireTimeInMinute));

                var apiToken = await GetApiTokenAsync(user, utcDateTime, refreshToken.RefreshReloginId, refreshTokenExpiryAt).ConfigureAwait(false);
                if (apiToken == null)
                {
                    responseResult.ErrorMessage.Add("Your role is not assigned. Please contact administrator.");
                    return responseResult;
                }

                var resetAccessFailedCountResult = await userManager.ResetAccessFailedCountAsync(user).ConfigureAwait(false);
                if (!resetAccessFailedCountResult.Succeeded && resetAccessFailedCountResult.Errors.Any())
                {
                    foreach (var error in resetAccessFailedCountResult.Errors)
                        responseResult.ErrorMessage.Add(error.Description);
                    return responseResult;
                }

                // create login log
                unitOfWork.LoginLogRepo.Create(new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    LoggedOn = utcDateTime,
                    IpAddress = modelDto.IpAddress
                });
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                responseResult.Data = new()
                {
                    ApiToken = apiToken.Token,
                    AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme,
                    RefreshEncodedToken = AesEndeCryptor.EncryptString(
                            EnDecryptionConstant.RefreshTokenKey,
                            EnDecryptionConstant.RefreshTokenIv,
                            JsonConvert.SerializeObject(new AuthRefreshEncodeDto { RefreshId = refreshToken.Id, ExpireAt = refreshTokenExpiryAt })
                            )
                };
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> ForgotPasswordAsync(ForgotPasswordDto modelDto)
        {
            var responseResult = new ResponseMessageDto<bool>();

            var user = await userManager.FindByEmailAsync(modelDto.Email).ConfigureAwait(false);
            if (user == null)
                responseResult.ErrorMessage.Add("Sorry! User does not found in our system.");
            else if (!user.CanLogin)
                responseResult.ErrorMessage.Add("You cannot login. Please contact administrator.");
            else if (!user.EmailConfirmed)
            {
                bool success = await SendEmailConfirmationAsync(user).ConfigureAwait(false);
                responseResult.ErrorMessage.Add("Email not confirmed yet! Please check your email account and gets email confirmed.");
                if (!success)
                    responseResult.ErrorMessage.Add("Problem in sending email verification link.");
            }
            else
            {
                var encryptUserId = resetPasswordProtector.Protect(user.Id, TimeSpan.FromMinutes(Convert.ToDouble(config["ResetPasswordLinkValidFromMinutes"])));
                var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                var tokenGeneratedBytes = Encoding.UTF8.GetBytes(resetPasswordToken);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var resetPasswordLink = $"{config["AppUrl"]}/resetpassword?encUserId={encryptUserId}&token={codeEncoded}";
                var success = await emailMessageService.PasswordResetLinkAsync(resetPasswordLink, user.Email, user.FirstName, user.LastName, logger).ConfigureAwait(false);
                if (!success)
                    responseResult.ErrorMessage.Add("Problem in sending reset password link.");
                else
                    responseResult.Data = true;
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> ResetPasswordAsync(ResetPasswordDto modelDto)
        {
            var responseResult = new ResponseMessageDto<bool>();

            try
            {
                var userId = resetPasswordProtector.Unprotect(modelDto.EncUserId);
                var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
                if (user == null)
                    responseResult.ErrorMessage.Add("Sorry! User does not found in our system.");
                else if (!user.CanLogin)
                    responseResult.ErrorMessage.Add("You cannot login. Please contact administrator.");
                else if (!user.EmailConfirmed)
                    responseResult.ErrorMessage.Add("Your email is not verified. Please contact administrator.");
                else
                {
                    // Remove Refresh Token and Forcing User to relogin
                    var isRefreshTokenDeleted = await RefreshTokenCommonService.DeleteByUserIdAsync(userId, unitOfWork, cache).ConfigureAwait(false);
                    if (isRefreshTokenDeleted || await unitOfWork.SaveChangesAsync().ConfigureAwait(false) > 0)
                    {
                        var codeDecodedBytes = WebEncoders.Base64UrlDecode(modelDto.Token);
                        var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                        var resetResult = await userManager.ResetPasswordAsync(user, codeDecoded, modelDto.Password).ConfigureAwait(false);
                        if (resetResult.Succeeded)
                        {
                            responseResult.Data = true;

                            var resetAccessFailedCountResult = await userManager.ResetAccessFailedCountAsync(user).ConfigureAwait(false);
                            if (!resetAccessFailedCountResult.Succeeded)
                                if (resetAccessFailedCountResult.Errors.Any())
                                    foreach (var error in resetAccessFailedCountResult.Errors)
                                        responseResult.ErrorMessage.Add(error.Description);
                        }
                        else
                            if (resetResult.Errors.Any())
                            foreach (var error in resetResult.Errors)
                                responseResult.ErrorMessage.Add(error.Description);
                    }
                    else
                        responseResult.ErrorMessage.Add("Failed to reset password.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while reseting password.");
                responseResult.ErrorMessage.Add("Error while reseting password.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> ConfirmEmailAsync(ConfirmEmailDto modelDto)
        {
            var responseResult = new ResponseMessageDto<bool>();

            try
            {
                var userId = confirmMailProtector.Unprotect(modelDto.EncUserId);
                var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
                if (user == null)
                    responseResult.ErrorMessage.Add("Sorry! User does not found in our system.");
                else if (!user.CanLogin)
                    responseResult.ErrorMessage.Add("You cannot login. Please contact administrator.");
                else
                {
                    var codeDecodedBytes = WebEncoders.Base64UrlDecode(modelDto.Token);
                    var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                    var resetResult = await userManager.ConfirmEmailAsync(user, codeDecoded).ConfigureAwait(false);
                    if (resetResult.Succeeded)
                    {
                        responseResult.Data = true;
                    }
                    else
                        if (resetResult.Errors.Any())
                        foreach (var error in resetResult.Errors)
                            responseResult.ErrorMessage.Add(error.Description);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while confirming email.");
                responseResult.ErrorMessage.Add("Error while confirming email.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> ChangePasswordAsync(ChangePasswordDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            if (modelDto.CurrentPassword == modelDto.Password)
            {
                responseResult.ErrorMessage.Add("Current Password and New Password cannot be same.");
                return responseResult;
            }

            var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
                responseResult.ErrorMessage.Add("Sorry! User does not found in our system.");
            else if (!user.CanLogin)
                responseResult.ErrorMessage.Add("You cannot login. Please contact administrator.");
            else if (!await userManager.CheckPasswordAsync(user, modelDto.CurrentPassword).ConfigureAwait(false))
                responseResult.ErrorMessage.Add("Current Password is not correct.");
            else if (!user.EmailConfirmed)
                responseResult.ErrorMessage.Add("Your email is not verified. Please contact administrator.");
            else
            {
                // Remove Refresh Token and Forcing User to relogin
                var isRefreshTokenDeleted = await RefreshTokenCommonService.DeleteByUserIdAsync(user.Id, unitOfWork, cache).ConfigureAwait(false);
                if (isRefreshTokenDeleted || await unitOfWork.SaveChangesAsync().ConfigureAwait(false) > 0)
                {
                    var resetResult = await userManager.ChangePasswordAsync(user, modelDto.CurrentPassword, modelDto.Password).ConfigureAwait(false);
                    if (resetResult.Succeeded)
                    {
                        responseResult.Data = true;

                        user.ChangePassword = false; // Changing ChangePassword to false
                        user.UpdatedOn = DateTime.UtcNow;
                        user.UpdatedById = userId;
                        user.IpAddress = modelDto.IpAddress;
                        var updateUserResult = await userManager.UpdateAsync(user).ConfigureAwait(false);
                        if (!updateUserResult.Succeeded)
                            if (updateUserResult.Errors.Any())
                                foreach (var error in updateUserResult.Errors)
                                    responseResult.ErrorMessage.Add(error.Description);
                    }
                    else
                        if (resetResult.Errors.Any())
                        foreach (var error in resetResult.Errors)
                            responseResult.ErrorMessage.Add(error.Description);
                }
                else
                    responseResult.ErrorMessage.Add("Failed to change password");
            }

            return responseResult;
        }
        public async Task<DataTableResponseDto<ApplicationUserDto>> GetAllAsync(ApplicationUserDataTableRequestDto modelDto)
        {
            var model = mapper.Map<ApplicationUserDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<ApplicationUserDto>
            {
                AaData = mapper.Map<List<ApplicationUserDto>>(await unitOfWork.ApplicationUserRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.ApplicationUserRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.ApplicationUserRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return await userManager.FindByEmailAsync(email).ConfigureAwait(false) == null;
        }

        public async Task<ResponseMessageDto<ApplicationUserDto>> RegisterUserAsync(RegisterApplicationUserDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<ApplicationUserDto>();

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var siteSetting = await unitOfWork.NextUserSettingRepo.GetWithLockFirstOrDefaultAsync().ConfigureAwait(false);
                if (siteSetting == null)
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    responseResult.ErrorMessage.Add("Site setting not found. Please contact administrator");
                    return responseResult;
                }

                var date = DateTime.UtcNow;

                var user = mapper.Map<ApplicationUser>(modelDto);
                user.CreatedById = userId;
                user.CreatedOn = date;
                user.ChangePassword = true;
                user.IsActive = true;
                user.CanLogin = true;
                user.UserName = randomService.GenerateUserName(user.FirstName, 3, siteSetting.NextUserNumber.ToString());

                var address = mapper.Map<Address>(modelDto.Address);
                address.CreatedById = userId;
                address.CreatedOn = date;
                address.IsActive = true;
                user.Addresses = new()
                {
                    address
                };

                siteSetting.NextUserNumber += 1;
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                var createResult = await userManager.CreateAsync(user).ConfigureAwait(false);
                await unitOfWork.CommitAsync().ConfigureAwait(false);

                if (createResult.Succeeded)
                {
                    responseResult.Data = mapper.Map<ApplicationUserDto>(user);
                    var addPasswordResult = await userManager.AddPasswordAsync(user, modelDto.Password).ConfigureAwait(false);
                    if (addPasswordResult.Succeeded)
                    {
                        bool success = await SendEmailConfirmationWithPasswordAsync(user, modelDto.Password).ConfigureAwait(false);
                        if (!success)
                            responseResult.ErrorMessage.Add("Problem in sending email verification link.");
                        _ = emailMessageService.NotifyUserRegistrationToAdministratorAsync(user.FirstName, user.LastName, user.UserName, logger).ConfigureAwait(false);
                    }
                    if (addPasswordResult.Errors.Any())
                        foreach (var error in addPasswordResult.Errors)
                            responseResult.ErrorMessage.Add(error.Description);
                }
                else if (createResult.Errors.Any())
                    foreach (var error in createResult.Errors)
                        responseResult.ErrorMessage.Add(error.Description);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Error while registering user.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<GetUpdateApplicationUserDto>> GetForEditAsync(string userId)
        {
            var responseResult = new ResponseMessageDto<GetUpdateApplicationUserDto>();

            var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User not found.");
                return responseResult;
            }
            responseResult.Data = mapper.Map<GetUpdateApplicationUserDto>(user);
            responseResult.Data.Address = mapper.Map<GetUpdateAddressDto>(user.Addresses.FirstOrDefault());

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateForEditAsync(string editUserId, UpdateApplicationUserDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var date = DateTime.UtcNow;

            var user = await userManager.FindByIdAsync(editUserId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User does not exist to update.");
                return responseResult;
            }
            mapper.Map(modelDto, user);
            user.UpdatedById = userId;
            user.UpdatedOn = date;

            var address = user.Addresses.FirstOrDefault();
            if (address == null)
            {
                address = mapper.Map<Address>(modelDto.Address);
                address.CreatedById = userId;
                address.CreatedOn = date;
                address.IsActive = true;
                address.UserId = editUserId;
            }
            else
            {
                address = mapper.Map(modelDto.Address, address);
                address.UpdatedById = userId;
                address.UpdatedOn = date;
            }
            user.Addresses = new()
            {
                address
            };

            var updateResult = await userManager.UpdateAsync(user).ConfigureAwait(false);
            if (updateResult.Succeeded)
                responseResult.Data = true;
            else if (updateResult.Errors.Any())
                foreach (var error in updateResult.Errors)
                    responseResult.ErrorMessage.Add(error.Description);

            return responseResult;
        }

        public async Task<ResponseMessageDto<GetProfileDto>> GetUserProfileAsync(string userId)
        {
            var responseResult = new ResponseMessageDto<GetProfileDto>();

            var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User not found.");
                return responseResult;
            }
            responseResult.Data = mapper.Map<GetProfileDto>(user);
            responseResult.Data.Address = mapper.Map<GetProfileAddressDto>(user.Addresses.FirstOrDefault());

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> ChangeEmailAsync(ChangeEmailDto modelDto, string userId)
        {
            var responseResult = new ResponseMessageDto<bool>();

            if (!await IsEmailAvailableAsync(modelDto.Email).ConfigureAwait(false))
            {
                responseResult.ErrorMessage.Add("Email is already in use.");
                return responseResult;
            }

            var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
                responseResult.ErrorMessage.Add("Sorry! User does not found in our system.");
            else
            {
                var changeEmailTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(config["ChangeEmailLinkValidFromMinutes"]));
                var encryptEmail = changeEmailProtector.Protect(modelDto.Email, changeEmailTimeSpan);
                var encryptUserId = changeEmailProtector.Protect(userId, changeEmailTimeSpan);
                var changeEmailToken = await userManager.GenerateChangeEmailTokenAsync(user, modelDto.Email).ConfigureAwait(false);
                var tokenGeneratedBytes = Encoding.UTF8.GetBytes(changeEmailToken);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var changeEmailLink = $"{config["AppUrl"]}/user/confirmchangeemail?encUserId={encryptUserId}&encEmail={encryptEmail}&token={codeEncoded}";
                var success = await emailMessageService.ChangeEmailLinkAsync(changeEmailLink, modelDto.Email, user.FirstName, user.LastName, logger).ConfigureAwait(false);
                if (!success)
                    responseResult.ErrorMessage.Add("Problem in sending change email link.");
                else
                    responseResult.Data = true;
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> ConfirmChangeEmailAsync(ConfirmChangeEmailDto modelDto)
        {
            var responseResult = new ResponseMessageDto<bool>();

            try
            {
                var userId = changeEmailProtector.Unprotect(modelDto.EncUserId);
                var email = changeEmailProtector.Unprotect(modelDto.EncEmail);
                var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
                if (user == null)
                    responseResult.ErrorMessage.Add("Sorry! User does not found in our system.");
                else if (!user.CanLogin)
                    responseResult.ErrorMessage.Add("You cannot login. Please contact administrator.");
                else
                {
                    var codeDecodedBytes = WebEncoders.Base64UrlDecode(modelDto.Token);
                    var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                    var resetResult = await userManager.ChangeEmailAsync(user, email, codeDecoded).ConfigureAwait(false);
                    if (resetResult.Succeeded)
                    {
                        responseResult.Data = true;
                    }
                    else
                        if (resetResult.Errors.Any())
                        foreach (var error in resetResult.Errors)
                            responseResult.ErrorMessage.Add(error.Description);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while changing email.");
                responseResult.ErrorMessage.Add("Error while changing email.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<GetApplicationUserPrivilegeDto>> GetUserPrivilegeAsync(string userId)
        {
            var responseResult = new ResponseMessageDto<GetApplicationUserPrivilegeDto>();

            var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User not found.");
                return responseResult;
            }

            responseResult.Data = mapper.Map<GetApplicationUserPrivilegeDto>(user);
            var roles = await roleManager.Roles.ToListAsync().ConfigureAwait(false);
            var claims = RoleConstant.GetApplicationPrivileges();

            var userRoles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            var userClaims = await userManager.GetClaimsAsync(user).ConfigureAwait(false);

            // Map roles
            responseResult.Data.UserRoles.AddRange(roles.Select(role => new GetUserRoleDto
            {
                Name = role.Name,
                IsSelected = userRoles.Any(x => x.Equals(role.Name, StringComparison.OrdinalIgnoreCase))
            }));

            // Map claims
            responseResult.Data.UserClaims.AddRange(claims.Select(claim => new GetUserClaimDto
            {
                RoleName = claim.Key,
                Policies = claim.Value.Select(claimType => new GetUserPolicyDto
                {
                    PolicyName = claimType.Key,
                    Claims = claimType.Value.Select(policy => new UserClaimDto
                    {
                        ClaimType = policy,
                        IsSelected = userClaims.Any(x => x.Type.Equals(policy, StringComparison.OrdinalIgnoreCase))
                    }).ToList()
                }).ToList()
            }));

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateUserPrivilegeAsync(UpdateApplicationUserPrivilegeDto modelDto, string updateUserId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            if (modelDto.RoleNames.Count == 0)
            {
                responseResult.ErrorMessage.Add("Please provide the role.");
                return responseResult;
            }

            var user = await userManager.FindByIdAsync(updateUserId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User does not exist to update privileges.");
                return responseResult;
            }

            if (updateUserId == userId) // means administrator
            {
                if (!modelDto.RoleNames.Contains(RoleConstant.Administrator))
                {
                    responseResult.ErrorMessage.Add("Cannot remove administrator role.");
                    return responseResult;
                }

                var claimsPolicyCount = RoleConstant.GetApplicationPrivileges()
                    .SelectMany(x => x.Value)
                    .SelectMany(x => x.Value).Count();
                if (modelDto.ClaimTypes.Count() != claimsPolicyCount)
                {
                    responseResult.ErrorMessage.Add("Please provide all policies.");
                    return responseResult;
                }
            }

            var userRoles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            var userClaims = await userManager.GetClaimsAsync(user).ConfigureAwait(false);

            var rolesToRemove = new List<string>();
            var rolesToAdd = new List<string>();
            foreach (var role in userRoles)
            {
                if (!modelDto.RoleNames.Contains(role))
                    rolesToRemove.Add(role);
            }
            foreach (var role in modelDto.RoleNames)
            {
                if (!userRoles.Contains(role))
                    rolesToAdd.Add(role);
            }

            var claimsToRemove = new List<Claim>();
            var claimsToAdd = new List<Claim>();
            foreach (var claim in userClaims)
            {
                if (!modelDto.ClaimTypes.Contains(claim.Type))
                    claimsToRemove.Add(claim);
            }
            foreach (var claim in modelDto.ClaimTypes)
            {
                if (!userClaims.Any(x => x.Type == claim))
                {
                    var newClaim = new Claim(claim, "true");
                    claimsToAdd.Add(newClaim);
                }
            }

            if (rolesToRemove.Count == 0 && rolesToAdd.Count == 0 && claimsToRemove.Count == 0 && claimsToAdd.Count == 0)
            {
                responseResult.ErrorMessage.Add("Nothing to update.");
                return responseResult;
            }

            var isRefreshTokenDeleted = await RefreshTokenCommonService.DeleteByUserIdAsync(user.Id, unitOfWork, cache).ConfigureAwait(false);
            if (!isRefreshTokenDeleted)
            {
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }

            if (rolesToRemove.Count != 0)
            {
                var rolesRemoveResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove).ConfigureAwait(false);
                if (rolesRemoveResult.Errors.Any())
                    foreach (var error in rolesRemoveResult.Errors)
                        responseResult.ErrorMessage.Add(error.Description);
            }
            if (rolesToAdd.Count != 0)
            {
                var rolesAddResult = await userManager.AddToRolesAsync(user, rolesToAdd).ConfigureAwait(false);
                if (rolesAddResult.Errors.Any())
                    foreach (var error in rolesAddResult.Errors)
                        responseResult.ErrorMessage.Add(error.Description);
            }

            if (claimsToRemove.Count != 0)
            {
                var claimsRemoveResult = await userManager.RemoveClaimsAsync(user, claimsToRemove).ConfigureAwait(false);
                if (claimsRemoveResult.Errors.Any())
                    foreach (var error in claimsRemoveResult.Errors)
                        responseResult.ErrorMessage.Add(error.Description);
            }
            if (claimsToAdd.Count != 0)
            {
                var claimsAddResult = await userManager.AddClaimsAsync(user, claimsToAdd).ConfigureAwait(false);
                if (claimsAddResult.Errors.Any())
                    foreach (var error in claimsAddResult.Errors)
                        responseResult.ErrorMessage.Add(error.Description);
            }

            if (responseResult.ErrorMessage.Count == 0)
                responseResult.Data = true;

            return responseResult;
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetAllByRolesAsync(IEnumerable<string> roles, string userId, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new List<ApplicationUserDto>();
            if (!isAdmin)
            {
                var entity = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
                responseResult.Add(mapper.Map<ApplicationUserDto>(entity));
                return responseResult;
            }

            var entities = await unitOfWork.ApplicationUserRepo.GetUsersByRolesAsync(roles).ConfigureAwait(false);
            responseResult.AddRange(mapper.Map<List<ApplicationUserDto>>(entities));

            return responseResult;
        }

        private async Task<bool> SendEmailConfirmationAsync(ApplicationUser user)
        {
            string confirmEmailLink = await GetEmailConfirmationLinkAsync(user).ConfigureAwait(false);
            bool success = await emailMessageService.EmailConfirmationLinkAsync(confirmEmailLink, user.Email, user.FirstName, user.LastName, logger).ConfigureAwait(false);
            return success;
        }

        private async Task<bool> SendEmailConfirmationWithPasswordAsync(ApplicationUser user, string password)
        {
            string confirmEmailLink = await GetEmailConfirmationLinkAsync(user).ConfigureAwait(false);
            bool success = await emailMessageService.EmailConfirmationLinkWithPasswordAsync(confirmEmailLink, user.Email, user.UserName, user.FirstName, user.LastName, password, logger).ConfigureAwait(false);
            return success;
        }

        private async Task<string> GetEmailConfirmationLinkAsync(ApplicationUser user)
        {
            var encryptUserId = confirmMailProtector.Protect(user.Id, TimeSpan.FromMinutes(Convert.ToDouble(config["ConfirmEmailLinkValidFromMinutes"])));
            var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
            var tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmEmailToken);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var confirmEmailLink = $"{config["AppUrl"]}/confirmemail?encUserId={encryptUserId}&token={codeEncoded}";
            return confirmEmailLink;
        }

        private async Task<ApiTokenDto> GetApiTokenAsync(ApplicationUser user, DateTime utcDateTime, string refreshReloginId, DateTime refreshTokenExpireyAt)
        {
            var userRoles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            if (userRoles == null || !userRoles.Any())
                return null;

            var principal = new ClaimsPrincipal();
            var claims = new List<Claim>
            {
                new ("id", user.Id),
                new ("changepassword", user.ChangePassword.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Iat, ToUnixEpochDate(utcDateTime).ToString(), ClaimValueTypes.Integer64),
                new ("expiryat", utcDateTime.Add(TimeSpan.FromMinutes(jwtSetting.TokenExpireTimeInMinute)).ToString("yyyy/MM/dd hh:mm:ss tt")),
                new ("refreshexpiryat", refreshTokenExpireyAt.ToString("yyyy/MM/dd hh:mm:ss tt")),
                new ("refreshreloginid", refreshReloginId)
            };
            var identity = new ClaimsIdentity(claims, "User Identity");
            principal.AddIdentity(identity);

            var claimRoles = new List<Claim>();
            foreach (var role in userRoles)
            {
                Claim claimRole = new(ClaimTypes.Role, role);
                claimRoles.Add(claimRole);
            }
            var identityRoles = new ClaimsIdentity(claimRoles, "User Roles");
            principal.AddIdentity(identityRoles);

            var userClaims = await userManager.GetClaimsAsync(user).ConfigureAwait(false);
            if (userClaims != null && userClaims.Any())
            {
                var claimUserClaims = new List<Claim>();
                foreach (var userClaim in userClaims)
                {
                    claimUserClaims.Add(userClaim);
                }
                var identityClaims = new ClaimsIdentity(claimUserClaims, "User Claims");
                principal.AddIdentity(identityClaims);
            }

            string audience;
            if (userRoles.Contains(RoleConstant.ShopInUser))
                audience = jwtSetting.Audience[3];
            else if (userRoles.Contains(RoleConstant.DotComSiteUser))
                audience = jwtSetting.Audience[2];
            else if (userRoles.Contains(RoleConstant.DotInSiteUser))
                audience = jwtSetting.Audience[1];
            else
                audience = jwtSetting.Audience[0];

            // Creating Token
            var jwt = new JwtSecurityToken(jwtSetting.Issuer, audience,
                principal.Claims, utcDateTime, utcDateTime.Add(TimeSpan.FromMinutes(jwtSetting.TokenExpireTimeInMinute)),
                jwtSetting.SigningCredentials);

            return new ApiTokenDto { Token = new JwtSecurityTokenHandler().WriteToken(jwt) };
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        public async Task<IEnumerable<ApplicationUserDto>> GetUsersByUserIdsAsync(IEnumerable<string> userIds)
        {
            var entities = await unitOfWork.ApplicationUserRepo.GetUsersByUserIdsAsync(userIds).ConfigureAwait(false);
            return mapper.Map<IEnumerable<ApplicationUserDto>>(entities);
        }
    }
}
