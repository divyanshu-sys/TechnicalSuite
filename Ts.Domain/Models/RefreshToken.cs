namespace Ts.Domain.Models
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string RefreshReloginId { get; set; }
    }
}
