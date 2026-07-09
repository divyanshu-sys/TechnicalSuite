using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotIn.Client.ViewModels.StoryVms
{
    public class UpdateStoryMainImageVm
    {
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Display(Name = "Main Image Name")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImage { get; set; }

        public string MainImageUrl { get; set; }

        [Display(Name = "Main Image Source")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImageSource { get; set; }

        [Display(Name = "Main Description")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainDescription { get; set; }
    }
}
