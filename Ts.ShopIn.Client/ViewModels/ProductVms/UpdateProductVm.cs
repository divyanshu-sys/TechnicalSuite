using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ProductVms
{
    public class UpdateProductVm
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(120, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [DisplayName("Is Available")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public bool IsAvailable { get; set; }
    }
}
