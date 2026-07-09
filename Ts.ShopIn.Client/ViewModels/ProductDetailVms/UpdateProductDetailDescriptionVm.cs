using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ProductDetailVms
{
    public class UpdateProductDetailDescriptionVm
    {
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Description { get; set; }
    }
}
