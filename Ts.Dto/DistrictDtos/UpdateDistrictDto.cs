using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.DistrictDtos
{
    public class UpdateDistrictDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int StateId { get; set; }
    }
}
