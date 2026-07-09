using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotCom.Client.ViewModels.StoryImageVms
{
    public class UpdateStoryImageVm
    {
        [Display(Name = "Image")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseChoose)]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Image Source")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string ImageSource { get; set; }
    }
}
