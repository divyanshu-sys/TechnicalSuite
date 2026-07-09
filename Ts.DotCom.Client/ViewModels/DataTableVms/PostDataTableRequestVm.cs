using System.ComponentModel;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.DotCom.Client.ViewModels.DataTableVms
{
    public class PostDataTableRequestVm : BaseDataTableRequestVm
    {
        [DisplayName("Post Worker")]
        public string PostWorkerId { get; set; }
        public DateTimeOffset? LastViewedOnStart { get; set; }
        public DateTimeOffset? LastViewedOnEnd { get; set; }
    }
}
