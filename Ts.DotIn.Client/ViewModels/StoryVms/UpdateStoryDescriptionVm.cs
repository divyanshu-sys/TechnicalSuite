using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotIn.Client.ViewModels.StoryVms
{
    public class UpdateStoryDescriptionVm
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(70, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(200, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Description { get; set; }

        [Display(Name = "Image Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Image { get; set; }

        [Display(Name = "Image Source")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string ImageSource { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public int Priority { get; set; }
    }
}
