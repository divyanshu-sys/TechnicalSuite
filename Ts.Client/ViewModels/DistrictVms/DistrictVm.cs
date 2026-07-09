using Ts.Client.ViewModels.StateVms;
namespace Ts.Client.ViewModels.DistrictVms
{
    public class DistrictVm : BaseVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public StateVm State { get; set; }
    }
}
