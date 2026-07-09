using Ts.Application.AppSettings;
namespace Ts.Application.Helpers.HelperClasses
{
    public class NotifierEventArgs : EventArgs
    {
        public SmsData Sms { get; set; }
        public EmailData Email { get; set; }
        public IEmailSetting EmailSetting { get; set; }
    }
}
