using System.ComponentModel;
using Ts.Client.ViewModels.DataTableVms;
namespace Ts.ShopIn.Client.ViewModels.DataTableVms
{
    public class ProductDataTableRequestVm : BaseDataTableRequestVm
    {
        [DisplayName("Product Worker")]
        public string ProductWorkerId { get; set; }
    }
}
