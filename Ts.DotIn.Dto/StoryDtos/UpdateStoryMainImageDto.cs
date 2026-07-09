using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.DotIn.Dto.StoryDtos
{
    public class UpdateStoryMainImageDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImage { get; set; }

        [SkipHtmlEncoding]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainImageSource { get; set; }

        [SkipHtmlEncoding]
        [MaxLength(200, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MainDescription { get; set; }
    }
}
