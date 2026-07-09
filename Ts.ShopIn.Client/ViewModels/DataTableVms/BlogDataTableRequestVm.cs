using System.ComponentModel;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.ShopIn.Client.ViewModels.DataTableVms
{
    public class BlogDataTableRequestVm : BaseDataTableRequestVm
    {
        [DisplayName("Blog Worker")]
        public string BlogWorkerId { get; set; }
        public DateTimeOffset? LastViewedOnStart { get; set; }
        public DateTimeOffset? LastViewedOnEnd { get; set; }
    }
}
