using Ts.Client.ViewModels;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.SubCategoryVms;
using Ts.Common.HelperExtensions;
using Ts.ShopIn.Client.ViewModels.BlogImageVms;
using Ts.ShopIn.Client.ViewModels.BlogViewVms;
namespace Ts.ShopIn.Client.ViewModels.BlogVms
{
    public class BlogVm : BaseVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BlogLink { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public string MetaDescription { get; set; }
        public int SubCategoryId { get; set; }
        public string Description { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string Keyword4 { get; set; }
        public string Keyword5 { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();
        public string PublishedById { get; set; }
        public string BlogWorkerId { get; set; }
        public string MainImageSource { get; set; }
        public string BlogSource { get; set; }

        public BlogImageVm BlogImage { get; set; }
        public BlogViewVm BlogView { get; set; }
        public SubCategoryVm SubCategory { get; set; }
        public ApplicationUserVm PublishedBy { get; set; }
        public ApplicationUserVm BlogWorker { get; set; }
    }
}
