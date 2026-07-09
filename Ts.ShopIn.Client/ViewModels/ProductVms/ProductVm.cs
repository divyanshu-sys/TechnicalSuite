using Ts.Client.ViewModels;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Common.HelperExtensions;
using Ts.ShopIn.Client.ViewModels.ProductImageVms;
namespace Ts.ShopIn.Client.ViewModels.ProductVms
{
    public class ProductVm : BaseVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedById { get; set; }
        public string ProductWorkerId { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();

        public ProductImageVm ProductImage { get; set; }
        public ApplicationUserVm PublishedBy { get; set; }
        public ApplicationUserVm ProductWorker { get; set; }
    }
}
