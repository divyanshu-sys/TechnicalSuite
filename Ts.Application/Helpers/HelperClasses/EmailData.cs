using System.Net.Mail;
using SendGridMail = SendGrid.Helpers.Mail;
namespace Ts.Application.Helpers.HelperClasses
{
    public class EmailData
    {
        public List<string> ToAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = true;
        public List<string> CcAddresses { get; set; }
        public List<string> BccAddresses { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<SendGridMail.Attachment> SendGridAttachments { get; set; }
    }
}
