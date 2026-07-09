using Microsoft.AspNetCore.Identity;
namespace Ts.ShopIn.Domain.Models
{
    public class ClientUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public int? GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfileImageName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
        public bool CanLogin { get; set; }

        public virtual ICollection<Cart> CartAddedByUsers { get; set; }
        public virtual ICollection<Order> OrderAddedByUsers { get; set; }
        public virtual RefreshToken CreatedRefreshToken { get; set; }
        public virtual ICollection<LoginLog> CreatedLoginLogs { get; set; }
    }
}
