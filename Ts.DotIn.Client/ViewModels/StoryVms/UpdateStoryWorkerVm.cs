using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotIn.Client.ViewModels.StoryVms
{
    public class UpdateStoryWorkerVm
    {
        public string Title { get; set; }

        [DisplayName("Story Worker")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string StoryWorkerId { get; set; }
    }
}
