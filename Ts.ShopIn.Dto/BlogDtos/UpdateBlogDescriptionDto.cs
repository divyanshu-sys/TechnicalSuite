using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.ShopIn.Dto.BlogDtos
{
    public class UpdateBlogDescriptionDto : BaseInputDto
    {
        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public string Description { get; set; }
    }
}
