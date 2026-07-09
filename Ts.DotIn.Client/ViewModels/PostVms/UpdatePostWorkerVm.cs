using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotIn.Client.ViewModels.PostVms
{
    public class UpdatePostWorkerVm
    {
        public string Title { get; set; }

        [DisplayName("Post Worker")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string PostWorkerId { get; set; }
    }
}
