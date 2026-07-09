using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using Ts.Application.AppInterfaces;
using Ts.Application.AppSettings;
using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.AppServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;
        private readonly ILogger<EmailService> logger;

        public EmailService(IConfiguration config,
            ILogger<EmailService> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public Task SendEmailNotificationEventAsync(object sender, NotifierEventArgs e)
        {
            if (config.GetValue<bool>("SendEmailFromSendGridClient"))
                _ = SendGridClientAsync(e.Email, e.EmailSetting, logger).ConfigureAwait(false);
            else
                _ = EmailAsync(e.Email, e.EmailSetting, logger).ConfigureAwait(false);

            return Task.CompletedTask;
        }

        public Task<bool> SendEmailAsync(EmailData emailData, IEmailSetting emailSetting, ILogger logger)
        {
            return EmailAsync(emailData, emailSetting, logger);
        }

        public Task<bool> SendEmailShopInAsync(EmailData emailData, IEmailSetting emailSetting, ILogger logger)
        {
            return EmailAsync(emailData, emailSetting, logger);
        }

        public Task<bool> SendGridClientAsync(EmailData emailData, IEmailSetting emailSetting, ILogger logger)
        {
            return EmailGridClientAsync(emailData, emailSetting, logger);
        }

        private static async Task<bool> EmailAsync(EmailData emailData, IEmailSetting emailSetting, ILogger logger)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailSetting.FromAddress, emailSetting.DisplayName);
            foreach (var toAddress in emailData.ToAddresses)
            {
                mailMessage.To.Add(toAddress);
            }

            if (emailData.CcAddresses != null && emailData.CcAddresses.Count != 0)
                foreach (var ccAddress in emailData.CcAddresses)
                {
                    mailMessage.CC.Add(ccAddress);
                }

            if (emailData.BccAddresses != null && emailData.BccAddresses.Count != 0)
                foreach (var bccAddress in emailData.BccAddresses)
                {
                    mailMessage.Bcc.Add(bccAddress);
                }

            if (emailData.Attachments != null && emailData.Attachments.Count != 0)
                foreach (var attachment in emailData.Attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }

            mailMessage.Subject = emailData.Subject;
            mailMessage.IsBodyHtml = emailData.IsBodyHtml;
            mailMessage.Body = emailData.Body;

            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.Host = emailSetting.Host;
                smtpClient.Port = emailSetting.Port;
                if (emailSetting.IsAuth)
                    smtpClient.Credentials = new NetworkCredential()
                    {
                        UserName = emailSetting.UserName,
                        Password = emailSetting.Password
                    };
                smtpClient.EnableSsl = emailSetting.Ssl;
                await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                return true;
            }
            catch (SmtpException ex)
            {
                logger.LogError(ex, "Error Sending Email - SmtpException occurred while sending email.");
                return false;
            }
        }

        private static async Task<bool> EmailGridClientAsync(EmailData emailData, IEmailSetting emailSetting, ILogger logger)
        {
            var client = new SendGridClient(emailSetting.ApiKey);
            var from = new EmailAddress(emailSetting.FromAddress, emailSetting.DisplayName);
            string subjects = emailData.Subject;
            List<EmailAddress> tos = new();
            foreach (var toAddress in emailData.ToAddresses)
            {
                var emailAddress = new EmailAddress(toAddress);
                tos.Add(emailAddress);
            }

            string plainTextContent = "";
            string htmlContent = "";
            if (emailData.IsBodyHtml)
                htmlContent = emailData.Body;
            else
                plainTextContent = emailData.Body;

            try
            {
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subjects, plainTextContent, htmlContent);
                if (emailData.CcAddresses != null && emailData.CcAddresses.Count != 0)
                {
                    List<EmailAddress> ccs = new();
                    ccs.AddRange(emailData.CcAddresses.Select(cc => new EmailAddress(cc)));
                    msg.AddCcs(ccs);
                }
                if (emailData.BccAddresses != null && emailData.BccAddresses.Count != 0)
                {
                    List<EmailAddress> bccs = new();
                    bccs.AddRange(emailData.BccAddresses.Select(bcc => new EmailAddress(bcc)));
                    msg.AddBccs(bccs);
                }
                if (emailData.SendGridAttachments != null && emailData.SendGridAttachments.Count != 0)
                    msg.AddAttachments(emailData.SendGridAttachments);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.Accepted)
                    return true;
                else
                    logger.LogError("Error Sending Email - Send Grid Client return status code - {StatusCode} with response message - {ResponseMessage}",
                        response.StatusCode, await response.Body.ReadAsStringAsync().ConfigureAwait(false));

                return false;
            }
            catch (SmtpException ex)
            {
                logger.LogError(ex, "Error Sending Email - SmtpException occurred while sending email via SendGrid.");
                return false;
            }
        }
    }
}
