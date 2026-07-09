using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;

namespace Ts.ShopIn.Dto.ProductDetailDocumentDtos
{
    public class UpdateProductDetailDocumentDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public IFormFile DocumentFile { get; set; }
    }
}
