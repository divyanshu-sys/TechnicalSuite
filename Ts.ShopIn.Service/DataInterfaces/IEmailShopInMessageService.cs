using Microsoft.Extensions.Logging;
using Ts.Dto;
namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IEmailShopInMessageService
    {
        Task<bool> EmailConfirmationLinkAsync(string confirmEmailLink, string toEmail, string firstName, string lastName, ILogger logger);

        Task<bool> PasswordResetLinkAsync(string resetPasswordLink, string toEmail, string firstName, string lastName, ILogger logger);

        Task<bool> ChangeEmailLinkAsync(string changeEmailLink, string toEmail, string firstName, string lastName, ILogger logger);

        Task<bool> NotifyUserRegistrationToAdministratorAsync(string firstName, string lastName, string userName, ILogger logger);

        Task<bool> NotifyContactFormPublicAsync(ContactFormDto contactFormDto, ILogger logger);

        Task<bool> NotifyOrderStatusAsync(string statusMessage, string orderNumber, string toEmail, string fullName, ILogger logger);
    }
}
