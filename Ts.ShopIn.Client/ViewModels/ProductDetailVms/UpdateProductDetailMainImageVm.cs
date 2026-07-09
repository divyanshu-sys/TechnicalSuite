using System.ComponentModel.DataAnnotations;
using Ts.Client.ViewModels.DeliveryPolicyVms;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ProductDetailVms
{
    public class UpdateProductDetailMainImageVm
    {
        public string Title { get; set; }

        [Display(Name = "Main Image Name")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImage { get; set; }

        public string MainImageUrl { get; set; }

        public DeliveryPolicyVm DeliveryPolicy { get; set; }
    }
}
