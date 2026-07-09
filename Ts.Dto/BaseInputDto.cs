using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.Dto
{
    public abstract class BaseInputDto
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(16, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string IpAddress { get; set; }
    }
}
