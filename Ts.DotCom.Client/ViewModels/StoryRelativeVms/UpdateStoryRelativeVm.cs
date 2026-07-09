using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotCom.Client.ViewModels.StoryRelativeVms
{
    public class UpdateStoryRelativeVm
    {
        [Display(Name = "HrefLang")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int HrefLangId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(500, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Href { get; set; }
    }
}
