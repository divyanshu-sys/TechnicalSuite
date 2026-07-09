using Ts.Client.ViewModels;

namespace Ts.ShopIn.Client.ViewModels.CartVms
{
    public class CartVm : BaseVm
    {
        public int ProductDetailId { get; set; }
        public int ItemCount { get; set; }
        public string UserId { get; set; }
    }
}
