using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ProductVms
{
    public class UpdateProductWorkerVm
    {
        public string Title { get; set; }

        [DisplayName("Product Worker")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string ProductWorkerId { get; set; }
    }
}
