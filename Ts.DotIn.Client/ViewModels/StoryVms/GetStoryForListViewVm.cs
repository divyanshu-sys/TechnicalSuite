using Ts.Common.HelperExtensions;
namespace Ts.DotIn.Client.ViewModels.StoryVms
{
    public class GetStoryForListViewVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StoryLink { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();
    }
}
