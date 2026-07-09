namespace Ts.Domain.Models
{
    public class LoginLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser LoggedInBy { get; set; }
        public DateTime LoggedOn { get; set; }
        public string IpAddress { get; set; }
    }
}
