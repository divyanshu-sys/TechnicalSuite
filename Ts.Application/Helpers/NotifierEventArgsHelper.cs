using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.Helpers
{
    public class NotifierEventArgsHelper
    {
        public NotifierEventArgs NotifierEventArgs { get; }

        public NotifierEventArgsHelper()
        {
            NotifierEventArgs = new NotifierEventArgs();
        }

        public NotifierEventArgsHelper SendEmail(List<string> toAddresses, string subject, string body, bool isBodyHtml)
        {
            var email = new EmailData
            {
                ToAddresses = toAddresses,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            NotifierEventArgs.Email = email;
            return this;
        }

        public NotifierEventArgsHelper SendSms(string messageText, List<string> mobileNos)
        {
            var sms = new SmsData
            {
                MessageText = messageText,
                MobileNos = mobileNos
            };
            NotifierEventArgs.Sms = sms;
            return this;
        }
    }
}
