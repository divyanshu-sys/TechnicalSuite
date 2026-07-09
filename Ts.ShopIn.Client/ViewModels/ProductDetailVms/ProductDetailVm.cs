using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.DeliveryPolicyVms;
using Ts.Client.ViewModels.ExchangePolicyVms;
using Ts.Client.ViewModels.ReturnPolicyVms;
using Ts.Client.ViewModels.ShopCategoryVms;
using Ts.Common.HelperExtensions;
using Ts.ShopIn.Client.ViewModels.ProductDetailDocumentVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailImageVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailViewVms;
using Ts.ShopIn.Client.ViewModels.ProductVariantVms;
namespace Ts.ShopIn.Client.ViewModels.ProductDetailVms
{
    public class ProductDetailVm : CurrencyBaseVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProductDetailLink { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public string MetaDescription { get; set; }
        public int ShopCategoryId { get; set; }
        public int ExchangePolicyId { get; set; }
        public int DeliveryPolicyId { get; set; }
        public int ReturnPolicyId { get; set; }
        public string Description { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string Keyword4 { get; set; }
        public string Keyword5 { get; set; }
        public bool IsAvailable { get; set; }
        public int Stock { get; set; }
        public string StockForView { get; set; }
        public decimal Mrp { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();
        public string PublishedById { get; set; }
        public string ProductDetailWorkerId { get; set; }
        public List<int> VariantOf { get; set; }

        public ProductVariantVm ProductVariant { get; set; }
        public ProductDetailImageVm ProductDetailImage { get; set; }
        public ProductDetailDocumentVm ProductDetailDocument { get; set; }
        public ProductDetailViewVm ProductDetailView { get; set; }
        public ShopCategoryVm ShopCategory { get; set; }
        public ApplicationUserVm PublishedBy { get; set; }
        public ApplicationUserVm ProductDetailWorker { get; set; }
        public ExchangePolicyVm ExchangePolicy { get; set; }
        public DeliveryPolicyVm DeliveryPolicy { get; set; }
        public ReturnPolicyVm ReturnPolicy { get; set; }
        public IEnumerable<GetProductDetailForListViewVm> ProductVariants { get; set; }
    }
}
