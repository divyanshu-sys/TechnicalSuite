using Ts.Application.AppInterfaces;
namespace Ts.Application.AppServices
{
    public class SmsMessageService : ISmsMessageService
    {
        private readonly ISmsService smsService;

        public SmsMessageService(ISmsService smsService)
        {
            this.smsService = smsService;
        }
    }
}
