using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ts.Application.AppInterfaces;
using Ts.Application.AppSettings;
using Ts.Application.HelperExtensions;
using Ts.Application.Helpers.HelperClasses;
using Ts.Dto;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.ShopIn.Service.DataServices
{
    public class EmailShopInMessageService : IEmailShopInMessageService
    {
        private readonly IConfiguration config;
        private readonly IEmailService emailService;
        private readonly IEmailSetting emailSetting;

        public EmailShopInMessageService(IConfiguration config,
            IOptions<EmailSettingShopIn> emailSetting,
            IEmailService emailService)
        {
            this.config = config;
            this.emailService = emailService;
            this.emailSetting = emailSetting.Value;
        }

        public Task<bool> EmailConfirmationLinkAsync(string confirmEmailLink, string toEmail, string firstName, string lastName, ILogger logger)
        {
            var emailData = new EmailData()
            {
                Subject = "Email Confirmation Link.",
                Body = $"<div>Dear {firstName} {lastName},<br /><br />Your email confirmation link is here," +
                    $" which is active only till {config["ConfirmEmailLinkValidFromMinutes"]} minute(s). Click to <a href=\"{confirmEmailLink}\">Verify</a>." +
                    $"<br /><br />Regards,<br />{emailSetting.DisplayName}</div>",
                ToAddresses = new() { toEmail },
                IsBodyHtml = true
            };
            return emailService.SendEmailAsync(emailData, emailSetting, logger);
        }

        public Task<bool> PasswordResetLinkAsync(string resetPasswordLink, string toEmail, string firstName, string lastName, ILogger logger)
        {
            var emailData = new EmailData()
            {
                Subject = "Request for Reset Password.",
                Body = $"<div>Dear {firstName} {lastName},<br /><br />Your password reset link is here," +
                    $" which is active only till {config["ResetPasswordLinkValidFromMinutes"]} minute(s). Click to <a href=\"{resetPasswordLink}\">Change</a>." +
                    $"<br /><br />Regards,<br />{emailSetting.DisplayName}</div>",
                ToAddresses = new() { toEmail },
                IsBodyHtml = true
            };
            return emailService.SendEmailAsync(emailData, emailSetting, logger);
        }

        public Task<bool> ChangeEmailLinkAsync(string changeEmailLink, string toEmail, string firstName, string lastName, ILogger logger)
        {
            var emailData = new EmailData()
            {
                Subject = "Request for Email Change.",
                Body = $"<div>Dear {firstName} {lastName},<br /><br />Your email change link is here," +
                    $" which is active only till {config["ChangeEmailLinkValidFromMinutes"]} minute(s). Click to <a href=\"{changeEmailLink}\">Change</a>." +
                    $"<br /><br />Regards,<br />{emailSetting.DisplayName}</div>",
                ToAddresses = new() { toEmail },
                IsBodyHtml = true
            };
            return emailService.SendEmailAsync(emailData, emailSetting, logger);
        }

        public Task<bool> NotifyUserRegistrationToAdministratorAsync(string firstName, string lastName, string userName, ILogger logger)
        {
            var emailData = new EmailData()
            {
                Subject = "User has registered.",
                Body = $"<div>Hi,<br /><br />User has been registered in the site.<br /><br />" +
                    $"Name: {firstName} {lastName}, UserName: {userName}" +
                    $"<br /><br />Regards,<br />{emailSetting.DisplayName}</div>",
                ToAddresses = new() { config["AdministratorEmail"] },
                IsBodyHtml = true
            };
            return emailService.SendEmailAsync(emailData, emailSetting, logger);
        }

        public Task<bool> NotifyContactFormPublicAsync(ContactFormDto contactFormDto, ILogger logger)
        {
            contactFormDto.HtmlEncodeObject();
            var emailData = new EmailData()
            {
                Subject = contactFormDto.Subject,
                Body = $"{contactFormDto.Message}<br /><br />From<br />{contactFormDto.Name}<br />{contactFormDto.Email}",
                ToAddresses = [config["ShopInEmail"]],
                IsBodyHtml = true
            };
            return emailService.SendEmailAsync(emailData, emailSetting, logger);
        }

        public Task<bool> NotifyOrderStatusAsync(string statusMessage, string orderNumber, string toEmail, string fullName, ILogger logger)
        {
            var emailData = new EmailData()
            {
                Subject = $"Status of order no. {orderNumber} - {emailSetting.DisplayName}",
                Body = $"<div>Dear {fullName},<br /><br />Order Number: {orderNumber}<br />Status: {statusMessage}" +
                    $"<br /><br />Go to <a href=\"{config["ShopInAppOrderListUrl"]}\">My Orders</a>." +
                    $"<br /><br />Regards,<br />{emailSetting.DisplayName}</div>",
                ToAddresses = new() { toEmail },
                IsBodyHtml = true
            };
            return emailService.SendEmailAsync(emailData, emailSetting, logger);
        }
    }
}
