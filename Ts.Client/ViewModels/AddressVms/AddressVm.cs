using System.ComponentModel.DataAnnotations;
using Ts.Client.ViewModels.PostOfficeVms;
namespace Ts.Client.ViewModels.AddressVms
{
    public class AddressVm : BaseVm
    {
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "State")]
        public int StateId { get; set; }

        [Display(Name = "District")]
        public int DistrictId { get; set; }

        [Display(Name = "PostOffice")]
        public int PostOfficeId { get; set; }
        public PostOfficeVm PostOffice { get; set; }
    }
}
