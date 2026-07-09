using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Client.ViewModels.ShopCategoryVms
{
    public class CreateShopCategoryVm
    {
        [DisplayName("Shop Category Name")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.LinkText, ErrorMessage = ErrorMessageConstant.LinkTextRegx)]
        public string Name { get; set; }
    }
}
