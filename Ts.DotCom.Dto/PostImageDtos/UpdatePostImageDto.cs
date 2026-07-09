using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.DotCom.Dto.PostImageDtos
{
    public class UpdatePostImageDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public IFormFile ImageFile { get; set; }

        [SkipHtmlEncoding]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string ImageSource { get; set; }
    }
}
