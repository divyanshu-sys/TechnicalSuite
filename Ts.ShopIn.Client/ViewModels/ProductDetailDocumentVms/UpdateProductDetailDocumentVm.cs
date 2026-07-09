using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;

namespace Ts.ShopIn.Client.ViewModels.ProductDetailDocumentVms
{
    public class UpdateProductDetailDocumentVm
    {
        [Display(Name = "Document")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseChoose)]
        public IFormFile DocumentFile { get; set; }
    }
}
