using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.BlogVms
{
    public class UpdateBlogMainImageVm
    {
        public string Title { get; set; }

        [Display(Name = "Main Image Name")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImage { get; set; }

        public string MainImageUrl { get; set; }

        [Display(Name = "Main Image Source")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImageSource { get; set; }
    }
}
