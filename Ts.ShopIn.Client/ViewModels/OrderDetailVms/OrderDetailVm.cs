using Ts.Client.ViewModels;
using Ts.Client.ViewModels.DeliveryPolicyVms;
using Ts.Client.ViewModels.OrderStatusVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;

namespace Ts.ShopIn.Client.ViewModels.OrderDetailVms
{
    public class OrderDetailVm : BaseVm
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public int? ProductDetailId { get; set; }
        public string Title { get; set; }
        public int ItemCount { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int ShopCategoryId { get; set; }
        public int ExchangePolicyId { get; set; }
        public int DeliveryPolicyId { get; set; }
        public int ReturnPolicyId { get; set; }

        public OrderStatusVm OrderStatus { get; set; }
        public CurrencyTypeVm CurrencyType { get; set; }
        public DeliveryPolicyVm DeliveryPolicy { get; set; }
        public GetProductDetailForListViewVm ProductDetailForView { get; set; }
        public bool IsDownloadable { get; set; }
        public int IsDownloadableRemainingDays { get; set; }
    }
}
