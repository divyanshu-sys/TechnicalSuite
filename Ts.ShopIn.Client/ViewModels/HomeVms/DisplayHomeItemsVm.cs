using Ts.ShopIn.Client.ViewModels.BlogVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;
using Ts.ShopIn.Client.ViewModels.ProductVms;

namespace Ts.ShopIn.Client.ViewModels.HomeVms
{
    public class DisplayHomeItemsVm
    {
        public IEnumerable<GetBlogForListViewVm> BlogForListView { get; set; }
        public IEnumerable<GetProductDetailForListViewVm> ProductDetailForListView { get; set; }
        public IEnumerable<GetProductForListViewVm> ProductForListView { get; set; }
    }
}
