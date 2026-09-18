using System.ComponentModel.DataAnnotations;
namespace Ts.Client.ViewModels.ClientUserVms
{
    public class ClientUserVm : BaseVm
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int? GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string ProfileImageName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
