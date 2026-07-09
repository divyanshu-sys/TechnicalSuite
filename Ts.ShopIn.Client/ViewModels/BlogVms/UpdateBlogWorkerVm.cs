using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.BlogVms
{
    public class UpdateBlogWorkerVm
    {
        public string Title { get; set; }

        [DisplayName("Blog Worker")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string BlogWorkerId { get; set; }
    }
}
