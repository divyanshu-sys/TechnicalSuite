using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.DotCom.Dto.StoryDtos
{
    public class UpdateStoryDescriptionDto : BaseInputDto
    {
        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(70, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(200, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Image { get; set; }

        [SkipHtmlEncoding]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string ImageSource { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public int Priority { get; set; }
    }
}
