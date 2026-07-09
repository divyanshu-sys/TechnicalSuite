using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DataTableDtos.ClientUserDataTableDtos;
using Ts.Dto.RefreshTokenDtos;
using Ts.ShopIn.Domain.DataTableModels.ClientUserDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.ClientUserDtos;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.ShopIn.Service.DataServices
{
    public class ClientUserService : IClientUserService
    {
        private readonly UserManager<ClientUser> userManager;
        private readonly IEmailShopInMessageService emailMessageService;
        private readonly IConfiguration config;
        private readonly JwtSettingShopIn jwtSetting;
        private readonly ITimeLimitedDataProtector confirmMailProtector;
        private readonly ITimeLimitedDataProtector changeEmailProtector;
        private readonly ITimeLimitedDataProtector resetPasswordProtector;
        private readonly ILogger<ClientUserService> logger;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRandomService randomService;

        public ClientUserService(UserManager<ClientUser> userManager, IEmailShopInMessageService emailMessageService,
            IOptions<JwtSettingShopIn> jwtSettings, IConfiguration config,
            IDataProtectionProvider dataProtectorProvider, ILogger<ClientUserService> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork, IRandomService randomService)
        {
            this.userManager = userManager;
            this.emailMessageService = emailMessageService;
            this.jwtSetting = jwtSettings.Value;
            this.config = config;
            this.logger = logger;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.randomService = randomService;
            confirmMailProtector = dataProtectorProvider.CreateProtector(DataProtectionPurposeStringConstant.EmailConfirmationShopInProtectionKey).ToTimeLimitedDataProtector();
            changeEmailProtector = dataProtectorProvider.CreateProtector(DataProtectionPurposeStringConstant.ChangeEmailShopInProtectionKey).ToTimeLimitedDataProtector();
            resetPasswordProtector = dataProtectorProvider.CreateProtector(DataProtectionPurposeStringConstant.ResetPasswordShopInProtectionKey).ToTimeLimitedDataProtector();
        }

        public async Task<ResponseMessageDto<ApiTokenDto>> GetRefreshApiTokenAsync(AuthRefreshDto modelDto)
        {
            var responseResult = new ResponseMessageDto<ApiTokenDto>();

            AuthRefreshEncodeDto authRefreshEncodeDto;
            try
            {
                var decodedRefreshToken = AesEndeCryptor.DecryptString(EnDecryptionConstant.RefreshTokenShopInKey, EnDecryptionConstant.RefreshTokenShopInIv, modelDto.EncodedRefreshToken);
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
                var apiToken = GetApiToken(user, DateTime.UtcNow, refreshToken.RefreshReloginId, authRefreshEncodeDto.ExpireAt.Value);
                responseResult.Data = apiToken;
                return responseResult;
            }

            responseResult.ErrorMessage.Add("Failed to get refreshed api token.");

            return responseResult;
        }

        public async Task<ResponseMessageDto<AuthTokenDto>> GetAuthenticationTokenAsync(LoginDto modelDto)
        {
            var responseResult = new ResponseMessageDto<AuthTokenDto>();

            ClientUser user;
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

                var apiToken = GetApiToken(user, utcDateTime, refreshToken.RefreshReloginId, refreshTokenExpiryAt);

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
                            EnDecryptionConstant.RefreshTokenShopInKey,
                            EnDecryptionConstant.RefreshTokenShopInIv,
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
                var resetPasswordLink = $"{config["ShopInAppUrl"]}/resetpassword?encUserId={encryptUserId}&token={codeEncoded}";
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
                    var isRefreshTokenDeleted = await RefreshTokenCommonService.DeleteByUserIdAsync(userId, unitOfWork).ConfigureAwait(false);
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
                var isRefreshTokenDeleted = await RefreshTokenCommonService.DeleteByUserIdAsync(user.Id, unitOfWork).ConfigureAwait(false);
                if (isRefreshTokenDeleted || await unitOfWork.SaveChangesAsync().ConfigureAwait(false) > 0)
                {
                    var resetResult = await userManager.ChangePasswordAsync(user, modelDto.CurrentPassword, modelDto.Password).ConfigureAwait(false);
                    if (resetResult.Succeeded)
                    {
                        responseResult.Data = true;

                        user.UpdatedOn = DateTime.UtcNow;
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
                    responseResult.ErrorMessage.Add("Failed to change password.");
            }

            return responseResult;
        }

        public async Task<DataTableResponseDto<ClientUserDto>> GetAllAsync(ClientUserDataTableRequestDto modelDto)
        {
            var model = mapper.Map<ClientUserDataTableRequest>(modelDto);
            var responseResult = new DataTableResponseDto<ClientUserDto>
            {
                AaData = mapper.Map<List<ClientUserDto>>(await unitOfWork.ClientUserRepo.GetAllAsync(model).ConfigureAwait(false)),
                RecordsTotal = await unitOfWork.ClientUserRepo.GetTotalCountAsync().ConfigureAwait(false)
            };
            if (!string.IsNullOrEmpty(modelDto.Search?.Value))
                responseResult.RecordsFiltered = await unitOfWork.ClientUserRepo.GetRecordsFilteredAsync(model).ConfigureAwait(false);
            else
                responseResult.RecordsFiltered = responseResult.RecordsTotal;
            return responseResult;
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return await userManager.FindByEmailAsync(email).ConfigureAwait(false) == null;
        }

        public async Task<ResponseMessageDto<ClientUserDto>> RegisterUserAsync(RegisterClientUserDto modelDto)
        {
            var responseResult = new ResponseMessageDto<ClientUserDto>();

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var siteSetting = await unitOfWork.NextUserSettingRepo.GetWithLockFirstOrDefaultAsync().ConfigureAwait(false);
                if (siteSetting == null)
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    responseResult.ErrorMessage.Add("Site setting not found. Please contact administrator.");
                    return responseResult;
                }

                var date = DateTime.UtcNow;

                var user = mapper.Map<ClientUser>(modelDto);
                user.CreatedOn = date;
                user.IsActive = true;
                user.CanLogin = true;
                user.UserName = randomService.GenerateUserName(user.FirstName, 3, siteSetting.NextUserNumber.ToString());

                siteSetting.NextUserNumber += 1;
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                var createResult = await userManager.CreateAsync(user).ConfigureAwait(false);
                await unitOfWork.CommitAsync().ConfigureAwait(false);

                if (createResult.Succeeded)
                {
                    responseResult.Data = mapper.Map<ClientUserDto>(user);
                    var addPasswordResult = await userManager.AddPasswordAsync(user, modelDto.Password).ConfigureAwait(false);
                    if (addPasswordResult.Succeeded)
                    {
                        bool success = await SendEmailConfirmationAsync(user).ConfigureAwait(false);
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

        public async Task<ResponseMessageDto<ClientUserDto>> GetForEditAsync(string userId)
        {
            var responseResult = new ResponseMessageDto<ClientUserDto>();

            var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User not found.");
                return responseResult;
            }
            responseResult.Data = mapper.Map<ClientUserDto>(user);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> UpdateForEditAsync(string editUserId, UpdateClientUserDto modelDto, string userId)
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
            user.UpdatedOn = date;

            var updateResult = await userManager.UpdateAsync(user).ConfigureAwait(false);
            if (updateResult.Succeeded)
                responseResult.Data = true;
            else if (updateResult.Errors.Any())
                foreach (var error in updateResult.Errors)
                    responseResult.ErrorMessage.Add(error.Description);

            return responseResult;
        }

        public async Task<ResponseMessageDto<ClientUserDto>> GetUserProfileAsync(string userId)
        {
            var responseResult = new ResponseMessageDto<ClientUserDto>();

            var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User not found.");
                return responseResult;
            }
            responseResult.Data = mapper.Map<ClientUserDto>(user);

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
                var changeEmailLink = $"{config["ShopInAppUrl"]}/confirm-change-email?encUserId={encryptUserId}&encEmail={encryptEmail}&token={codeEncoded}";
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
                logger.LogWarning(ex, "Error while changing email.");
                responseResult.ErrorMessage.Add("Error while changing email.");
            }

            return responseResult;
        }

        private async Task<bool> SendEmailConfirmationAsync(ClientUser user)
        {
            string confirmEmailLink = await GetEmailConfirmationLinkAsync(user).ConfigureAwait(false);
            bool success = await emailMessageService.EmailConfirmationLinkAsync(confirmEmailLink, user.Email, user.FirstName, user.LastName, logger).ConfigureAwait(false);
            return success;
        }

        private async Task<string> GetEmailConfirmationLinkAsync(ClientUser user)
        {
            var encryptUserId = confirmMailProtector.Protect(user.Id, TimeSpan.FromMinutes(Convert.ToDouble(config["ConfirmEmailLinkValidFromMinutes"])));
            var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
            var tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmEmailToken);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var confirmEmailLink = $"{config["ShopInAppUrl"]}/confirmemail?encUserId={encryptUserId}&token={codeEncoded}";
            return confirmEmailLink;
        }

        private ApiTokenDto GetApiToken(ClientUser user, DateTime utcDateTime, string refreshReloginId, DateTime refreshTokenExpireyAt)
        {
            var principal = new ClaimsPrincipal();
            var claims = new List<Claim>
            {
                new ("id", user.Id),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Iat, ToUnixEpochDate(utcDateTime).ToString(), ClaimValueTypes.Integer64),
                new ("expiryat", utcDateTime.Add(TimeSpan.FromMinutes(jwtSetting.TokenExpireTimeInMinute)).ToString("yyyy/MM/dd hh:mm:ss tt")),
                new ("refreshexpiryat", refreshTokenExpireyAt.ToString("yyyy/MM/dd hh:mm:ss tt")),
                new ("refreshreloginid", refreshReloginId)
            };
            var identity = new ClaimsIdentity(claims, "User Identity");
            principal.AddIdentity(identity);

            // Creating Token
            var jwt = new JwtSecurityToken(jwtSetting.Issuer, jwtSetting.Audience[0],
                principal.Claims, utcDateTime, utcDateTime.Add(TimeSpan.FromMinutes(jwtSetting.TokenExpireTimeInMinute)),
                jwtSetting.SigningCredentials);

            return new ApiTokenDto { Token = new JwtSecurityTokenHandler().WriteToken(jwt) };
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}
