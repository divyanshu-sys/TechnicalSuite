using Ts.Client.ViewModels.DistrictVms;
namespace Ts.Client.ViewModels.PostOfficeVms
{
    public class PostOfficeVm : BaseVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pincode { get; set; }
        public int DistrictId { get; set; }
        public DistrictVm District { get; set; }
    }
}
