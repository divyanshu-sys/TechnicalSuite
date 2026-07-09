using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.DotCom.Dto.StoryRelativeDtos
{
    public class UpdateStoryRelativeDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int HrefLangId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(500, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Href { get; set; }
    }
}
