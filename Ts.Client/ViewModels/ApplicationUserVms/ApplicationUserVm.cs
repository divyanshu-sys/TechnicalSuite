using System.ComponentModel.DataAnnotations;
using Ts.Client.ViewModels.AddressVms;
namespace Ts.Client.ViewModels.ApplicationUserVms
{
    public class ApplicationUserVm : BaseVm
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public int? GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ProfileImageName { get; set; }
        public string PhoneNumber { get; set; }
        public AddressVm Address { get; set; }
    }
}
