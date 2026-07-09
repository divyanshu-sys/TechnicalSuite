using Ts.Common.Constant.SiteConstants;
using Ts.Dto;

namespace Ts.ShopIn.Dto
{
    public abstract class CurrencyBaseDto : BaseDto
    {
        public string Currency { get; set; } = CurrencyTypeConstant.INRSymbol;
        public string CurrencyLetter { get; set; } = CurrencyTypeConstant.INR;
    }
}
