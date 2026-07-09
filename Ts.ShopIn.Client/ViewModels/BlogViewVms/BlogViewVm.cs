using Ts.Common.HelperExtensions;
namespace Ts.ShopIn.Client.ViewModels.BlogViewVms
{
    public class BlogViewVm
    {
        public int Id { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }
        public string LastViewedOnIst => LastViewedOn.ToDateTimeIstString();
    }
}
