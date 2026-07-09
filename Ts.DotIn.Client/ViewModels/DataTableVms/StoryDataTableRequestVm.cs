using System.ComponentModel;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.DotIn.Client.ViewModels.DataTableVms
{
    public class StoryDataTableRequestVm : BaseDataTableRequestVm
    {
        [DisplayName("Story Worker")]
        public string StoryWorkerId { get; set; }
    }
}
