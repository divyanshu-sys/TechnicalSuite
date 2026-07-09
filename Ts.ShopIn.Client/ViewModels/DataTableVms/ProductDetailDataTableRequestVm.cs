using System.ComponentModel;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.ShopIn.Client.ViewModels.DataTableVms
{
    public class ProductDetailDataTableRequestVm : BaseDataTableRequestVm
    {
        [DisplayName("Product Detail Worker")]
        public string ProductDetailWorkerId { get; set; }
        public DateTimeOffset? LastViewedOnStart { get; set; }
        public DateTimeOffset? LastViewedOnEnd { get; set; }
    }
}
