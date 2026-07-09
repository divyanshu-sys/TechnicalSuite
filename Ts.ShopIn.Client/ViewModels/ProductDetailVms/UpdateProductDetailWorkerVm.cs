using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ProductDetailVms
{
    public class UpdateProductDetailWorkerVm
    {
        public string Title { get; set; }

        [DisplayName("Product Detail Worker")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string ProductDetailWorkerId { get; set; }
    }
}
