using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotCom.Client.ViewModels.PostVms
{
    public class UpdatePostDescriptionVm
    {
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Description { get; set; }
    }
}
