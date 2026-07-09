using Ts.Common.HelperExtensions;
namespace Ts.ShopIn.Client.ViewModels.ProductDetailViewVms
{
    public class ProductDetailViewVm
    {
        public int Id { get; set; }
        public int TotalViews { get; set; }
        public DateTime LastViewedOn { get; set; }
        public string LastViewedOnIst => LastViewedOn.ToDateTimeIstString();
    }
}
