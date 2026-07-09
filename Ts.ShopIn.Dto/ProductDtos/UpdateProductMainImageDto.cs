using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductDtos
{
    public class UpdateProductMainImageDto : BaseInputDto
    {
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImage { get; set; }
    }
}
