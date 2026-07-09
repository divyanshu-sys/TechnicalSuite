using Ts.Common.HelperExtensions;
namespace Ts.ShopIn.Client.ViewModels.ProductVms
{
    public class GetProductForListViewVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ShopCategoryId { get; set; }
        public string ShopCategoryName { get; set; }
        public string ProductDetailLink { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();
    }
}
