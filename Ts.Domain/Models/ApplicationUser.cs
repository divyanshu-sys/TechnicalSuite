using Microsoft.AspNetCore.Identity;
namespace Ts.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public int? GenderId { get; set; }
        public virtual Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfileImageName { get; set; }
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool ChangePassword { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
        public bool CanLogin { get; set; }

        // Add Navigation properties below
        public virtual ICollection<Country> CreatedCountries { get; set; }
        public virtual ICollection<Country> UpdatedCountries { get; set; }
        public virtual ICollection<ApplicationUser> CreatedApplicationUsers { get; set; }
        public virtual ICollection<ApplicationUser> UpdatedApplicationUsers { get; set; }
        public virtual RefreshToken CreatedRefreshToken { get; set; }
        public virtual ICollection<LoginLog> CreatedLoginLogs { get; set; }
        public virtual ICollection<State> CreatedStates { get; set; }
        public virtual ICollection<State> UpdatedStates { get; set; }
        public virtual ICollection<District> CreatedDistricts { get; set; }
        public virtual ICollection<District> UpdatedDistricts { get; set; }
        public virtual ICollection<PostOffice> CreatedPostOffices { get; set; }
        public virtual ICollection<PostOffice> UpdatedPostOffices { get; set; }
        public virtual ICollection<Address> CreatedAddresses { get; set; }
        public virtual ICollection<Address> UpdatedAddresses { get; set; }
        public virtual List<Address> Addresses { get; set; }
        public virtual ICollection<Category> CreatedCategories { get; set; }
        public virtual ICollection<Category> UpdatedCategories { get; set; }
        public virtual ICollection<SubCategory> CreatedSubCategories { get; set; }
        public virtual ICollection<SubCategory> UpdatedSubCategories { get; set; }
        public virtual ICollection<ShopCategory> CreatedShopCategories { get; set; }
        public virtual ICollection<ShopCategory> UpdatedShopCategories { get; set; }
    }
}
