namespace Ts.ShopIn.Domain.Models
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual ClientUser User { get; set; }
        public string RefreshReloginId { get; set; }
    }
}
