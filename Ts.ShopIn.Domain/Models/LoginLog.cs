namespace Ts.ShopIn.Domain.Models
{
    public class LoginLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual ClientUser LoggedInBy { get; set; }
        public DateTime LoggedOn { get; set; }
        public string IpAddress { get; set; }
    }
}
