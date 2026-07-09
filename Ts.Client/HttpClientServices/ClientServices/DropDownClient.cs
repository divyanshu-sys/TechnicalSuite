using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class DropDownClient : IDropDownClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly string BaseUrl;

        public DropDownClient(IHttpClientService httpClientService, IConfiguration configuration)
        {
            this.httpClientService = httpClientService;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetHrefLangAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<DropdownItemDto>>($"{BaseUrl}/{ApiUrl}/dropdown/hreflang", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetExchangePolicyDropDownAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<DropdownItemDto>>($"{BaseUrl}/{ApiUrl}/dropdown/exchangepolicy", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetDeliveryPolicyDropDownAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<DropdownItemDto>>($"{BaseUrl}/{ApiUrl}/dropdown/deliverypolicy", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetReturnPolicyDropDownAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<DropdownItemDto>>($"{BaseUrl}/{ApiUrl}/dropdown/returnpolicy", true).ConfigureAwait(false);
        }
    }
}
