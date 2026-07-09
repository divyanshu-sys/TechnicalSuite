using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Dto;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.CartVms;
using Ts.ShopIn.Dto.CartDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class CartClient : ICartClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientUserService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public CartClient(IHttpClientUserService httpClientService,
            IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<GetCartForListViewDto>> GetByUserAsync()
        {
            return await httpClientService.GetAsync<GetCartForListViewDto>($"{BaseUrl}/{ApiUrl}/cartdotin/getbyuser", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int productDetailId)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/cartdotin/{productDetailId}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<CartVm>> PostAsync(CreateCartVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateCartDto>(model);
            modelDto.IpAddress = ipAddress;

            return await httpClientService.PostAsync<CartVm>($"{BaseUrl}/{ApiUrl}/cartdotin", modelDto, true).ConfigureAwait(false);
        }
    }
}
