using Microsoft.Extensions.Logging;
using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.AppInterfaces
{
    public interface ISmsService
    {
        Task<SmsResponse> SendSmsAsync(SmsData smsData, ILogger logger);

        Task SendSmsNotificationAsync(object sender, NotifierEventArgs e);
    }

    public class SmsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
