using Ts.Client.ViewModels.CountryVms;
namespace Ts.Client.ViewModels.StateVms
{
    public class StateVm : BaseVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code2 { get; set; }
        public int CountryId { get; set; }
        public CountryVm Country { get; set; }
    }
}
