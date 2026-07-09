using Ts.Common.HelperExtensions;
namespace Ts.DotIn.Client.ViewModels.PostVms
{
    public class GetPostForListViewVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PostLink { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();
    }
}
