using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.DotIn.Dto.PostRelativeDtos
{
    public class UpdatePostRelativeDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int HrefLangId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(500, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Href { get; set; }
    }
}
