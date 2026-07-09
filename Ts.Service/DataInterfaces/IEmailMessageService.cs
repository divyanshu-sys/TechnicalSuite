using Microsoft.Extensions.Logging;
using Ts.Dto;
namespace Ts.Service.DataInterfaces
{
    public interface IEmailMessageService
    {
        Task<bool> EmailConfirmationLinkAsync(string confirmEmailLink, string toEmail, string firstName, string lastName, ILogger logger);

        Task<bool> PasswordResetLinkAsync(string resetPasswordLink, string toEmail, string firstName, string lastName, ILogger logger);

        Task<bool> ChangeEmailLinkAsync(string changeEmailLink, string toEmail, string firstName, string lastName, ILogger logger);

        Task<bool> EmailConfirmationLinkWithPasswordAsync(string confirmEmailLink, string toEmail, string userName, string firstName, string lastName, string password, ILogger logger);

        Task<bool> NotifyUserRegistrationToAdministratorAsync(string firstName, string lastName, string userName, ILogger logger);

        Task<bool> NotifyContactFormPublicAsync(ContactFormDto contactFormDto, ILogger logger);
    }
}
