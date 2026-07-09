using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Ts.Application.AppInterfaces;
using Ts.Application.AppSettings;
using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.AppServices
{
    public class SmsService : ISmsService
    {
        private readonly SmsSetting smsSetting;
        private readonly ILogger<SmsService> logger;

        public SmsService(IOptions<SmsSetting> smsSetting,
            ILogger<SmsService> logger)
        {
            this.smsSetting = smsSetting.Value;
            this.logger = logger;
        }

        public Task SendSmsNotificationAsync(object sender, NotifierEventArgs e)
        {
            _ = SmsAsync(e.Sms, logger).ConfigureAwait(false);

            return Task.CompletedTask;
        }

        public Task<SmsResponse> SendSmsAsync(SmsData smsData, ILogger logger)
        {
            return SmsAsync(smsData, logger);
        }

        private async Task<SmsResponse> SmsAsync(SmsData smsData, ILogger logger)
        {
            string baseUrl;
            if (smsData.MobileNos.Count > 1)
                baseUrl = $"{smsSetting.Url}?username={smsSetting.Username}&password={smsSetting.Password}&to={string.Join(",", smsData.MobileNos.Select(x => $"91{x}"))}&from={smsSetting.SenderId}&text={smsData.MessageText}&category=bulk";
            else
                baseUrl = $"{smsSetting.Url}?username={smsSetting.Username}&password={smsSetting.Password}&to={smsData.MobileNos[0]}&from={smsSetting.SenderId}&text={smsData.MessageText}";

            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(baseUrl).ConfigureAwait(false);
            var smsResponse = new SmsResponse
            {
                Message = await response.Content.ReadAsStringAsync().ConfigureAwait(false)
            };
            if (response.StatusCode == HttpStatusCode.OK)
                smsResponse.IsSuccess = true;
            else
            {
                var responseMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                logger.LogError(
                    "Error Sending SMS - return status code - {StatusCode} with response message - {ResponseMessage}",
                    response.StatusCode,
                    responseMessage
                );
                smsResponse.IsSuccess = false;
            }

            return smsResponse;
        }
    }
}
