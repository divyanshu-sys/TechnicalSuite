using Ts.Client.ViewModels;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.CategoryVms;
using Ts.Client.ViewModels.SubCategoryVms;
using Ts.Common.HelperExtensions;
using Ts.DotCom.Client.ViewModels.PostImageVms;
using Ts.DotCom.Client.ViewModels.PostRelativeVms;
using Ts.DotCom.Client.ViewModels.PostViewVms;
namespace Ts.DotCom.Client.ViewModels.PostVms
{
    public class PostVm : BaseVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PostLink { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public string MetaDescription { get; set; }
        public int CategoryId { get; set; }
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
        public string PublishedOnCst => PublishedOn?.ToDateTimeCstString();
        public string PublishedById { get; set; }
        public string PostWorkerId { get; set; }
        public string MainImageSource { get; set; }
        public string PostSource { get; set; }

        public PostImageVm PostImage { get; set; }
        public PostViewVm PostView { get; set; }
        public CategoryVm Category { get; set; }
        public SubCategoryVm SubCategory { get; set; }
        public ApplicationUserVm PublishedBy { get; set; }
        public ApplicationUserVm PostWorker { get; set; }
        public PostRelativeVm PostRelative { get; set; }
    }
}
