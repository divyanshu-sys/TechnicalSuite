using Ts.Common.HelperExtensions;
namespace Ts.DotIn.Client.ViewModels.PostViewVms
{
    public class PostViewVm
    {
        public int Id { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }
        public string LastViewedOnIst => LastViewedOn.ToDateTimeIstString();
    }
}
