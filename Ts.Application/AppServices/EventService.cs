using Ts.Application.AppInterfaces;
using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.AppServices
{
    public class EventService : IEventService
    {
        private readonly IEmailService emailService;
        private readonly ISmsService smsService;

        public EventService(IEmailService emailService, ISmsService smsService)
        {
            this.emailService = emailService;
            this.smsService = smsService;
        }

        public delegate Task NotificationEventHandler(object sender, NotifierEventArgs e);

        public event NotificationEventHandler NotificationEvent;

        private void RaiseEvent(NotifierEventArgs e)
        {
            NotificationEvent?.Invoke(this, e);
        }

        public void SendNotificationsAsync(NotifierEventArgs e)
        {
            NotificationEvent = emailService.SendEmailNotificationEventAsync;
            NotificationEvent += smsService.SendSmsNotificationAsync;
            RaiseEvent(e);
        }
    }
}
