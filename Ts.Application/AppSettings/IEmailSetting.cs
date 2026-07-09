namespace Ts.Application.AppSettings
{
    public class IEmailSetting
    {
        public string Host { get; set; }
        public bool Ssl { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public bool IsAuth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string ApiKey { get; set; }
    }
}
