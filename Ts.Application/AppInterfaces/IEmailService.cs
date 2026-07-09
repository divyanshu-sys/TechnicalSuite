using Microsoft.Extensions.Logging;
using Ts.Application.AppSettings;
using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.AppInterfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailData emailData, IEmailSetting emailSetting, ILogger logger);

        Task<bool> SendGridClientAsync(EmailData emailData, IEmailSetting emailSetting, ILogger logger);

        Task SendEmailNotificationEventAsync(object sender, NotifierEventArgs e);
    }
}
