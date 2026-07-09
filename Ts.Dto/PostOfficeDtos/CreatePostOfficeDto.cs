using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto.PostOfficeDtos
{
    public class CreatePostOfficeDto : BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = ErrorMessageConstant.StringLength)]
        public string Pincode { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSelect)]
        public int DistrictId { get; set; }
    }
}
