using Ts.Common.HelperExtensions;
namespace Ts.ShopIn.Client.ViewModels.BlogVms
{
    public class GetBlogForListViewVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BlogLink { get; set; }
        public string MainImageUrl { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();
    }
}
