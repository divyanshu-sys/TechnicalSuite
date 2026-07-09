using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ProductImageVms
{
    public class UpdateProductImageVm
    {
        [Display(Name = "Image")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseChoose)]
        public IFormFile ImageFile { get; set; }
    }
}
