using Ts.Dto;
using Ts.Dto.CurrencyTypeDtos;
using Ts.Dto.DeliveryPolicyDtos;
using Ts.Dto.ExchangePolicyDtos;
using Ts.Dto.OrderStatusDtos;
using Ts.Dto.PaymentGatewayTypeDtos;
using Ts.Dto.PaymentModeDtos;
using Ts.Dto.PaymentStatusDtos;
using Ts.Dto.ReturnPolicyDtos;

namespace Ts.Service.DataInterfaces
{
    public interface IDropDownService
    {
        Task<IEnumerable<ExchangePolicyDto>> GetExchangePoliciesAsync();
        Task<IEnumerable<DropdownItemDto>> GetExchangePolicyDropDownAsync();

        Task<IEnumerable<DeliveryPolicyDto>> GetDeliveryPoliciesAsync();
        Task<IEnumerable<DropdownItemDto>> GetDeliveryPolicyDropDownAsync();

        Task<IEnumerable<ReturnPolicyDto>> GetReturnPoliciesAsync();
        Task<IEnumerable<DropdownItemDto>> GetReturnPolicyDropDownAsync();

        Task<IEnumerable<PaymentModeDto>> GetPaymentModesAsync();

        Task<IEnumerable<OrderStatusDto>> GetOrderStatusesAsync();

        Task<IEnumerable<PaymentGatewayTypeDto>> GetPaymentGatewayTypesAsync();

        Task<IEnumerable<PaymentStatusDto>> GetPaymentStatusesAsync();

        Task<IEnumerable<CurrencyTypeDto>> GetCurrencyTypesAsync();

        IEnumerable<DropdownItemDto> GetHrefLangs();
    }
}
