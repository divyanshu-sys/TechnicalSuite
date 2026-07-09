using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface IDropDownClient
    {
        Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetHrefLangAsync();
        Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetExchangePolicyDropDownAsync();
        Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetDeliveryPolicyDropDownAsync();
        Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetReturnPolicyDropDownAsync();
    }
}
