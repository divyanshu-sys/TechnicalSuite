using Newtonsoft.Json;
using Ts.Client.ViewModels;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.SubCategoryVms;
using Ts.Common.HelperExtensions;
using Ts.DotCom.Client.ViewModels.StoryImageVms;
using Ts.DotCom.Client.ViewModels.StoryRelativeVms;
using Ts.DotCom.Client.ViewModels.StoryViewVms;
namespace Ts.DotCom.Client.ViewModels.StoryVms
{
    public class StoryVm : BaseVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StoryLink { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public string ImageBaseUrl { get; set; }
        public string MetaDescription { get; set; }
        public string MainDescription { get; set; }
        public int SubCategoryId { get; set; }
        public string Description { get; set; }
        public List<StoryDescriptionHelperVm> StoryDescriptionHelperVms => JsonConvert.DeserializeObject<IEnumerable<StoryDescriptionHelperVm>>(Description ?? "[]").OrderBy(x => x.Priority).ToList();
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string Keyword4 { get; set; }
        public string Keyword5 { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedOnIst => PublishedOn?.ToDateTimeIstString();
        public string PublishedOnCst => PublishedOn?.ToDateTimeCstString();
        public string PublishedById { get; set; }
        public string StoryWorkerId { get; set; }
        public string MainImageSource { get; set; }
        public string StorySource { get; set; }

        public StoryImageVm StoryImage { get; set; }
        public StoryViewVm StoryView { get; set; }
        public SubCategoryVm SubCategory { get; set; }
        public ApplicationUserVm PublishedBy { get; set; }
        public ApplicationUserVm StoryWorker { get; set; }
        public StoryRelativeVm StoryRelative { get; set; }
    }

    public class StoryDescriptionHelperVm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ImageSource { get; set; }
        public int Priority { get; set; }
    }
}
