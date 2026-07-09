using Ts.Common.HelperExtensions;
namespace Ts.DotIn.Client.ViewModels.StoryViewVms
{
    public class StoryViewVm
    {
        public int Id { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }
        public string LastViewedOnIst => LastViewedOn.ToDateTimeIstString();
    }
}
