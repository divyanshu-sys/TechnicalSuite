using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;

namespace Ts.ShopIn.Client.ViewModels.CartVms
{
    public class CreateCartVm
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ProductDetailId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public int ItemCount { get; set; }
    }
}
