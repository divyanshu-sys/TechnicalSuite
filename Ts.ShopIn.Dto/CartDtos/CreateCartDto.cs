using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;

namespace Ts.ShopIn.Dto.CartDtos
{
    public class CreateCartDto : BaseInputDto
    {

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ProductDetailId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public int ItemCount { get; set; }
    }
}
