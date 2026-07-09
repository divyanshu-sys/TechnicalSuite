using Microsoft.Extensions.Configuration;
using Ts.Dto;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.OrderVms;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;
using Ts.ShopIn.Dto.OrderDtos;
using Ts.ShopIn.Dto.PaymentDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class OrderForViewClient : IOrderForViewClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientUserService httpClientService;
        private readonly string BaseUrl;

        public OrderForViewClient(IHttpClientUserService httpClientService,
            IConfiguration configuration)
        {
            this.httpClientService = httpClientService;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<GetRazorpayOrderDto>> CreateRazorpayOrderAsync()
        {
            return await httpClientService.PostAsync<GetRazorpayOrderDto>($"{BaseUrl}/{ApiUrl}/orderdotin/create-razorpay-order", null, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<OrderVm>>> GetByUserAsync(OrderDataTableForViewRequestDto model)
        {
            return await httpClientService.PostAsync<IEnumerable<OrderVm>>($"{BaseUrl}/{ApiUrl}/orderdotin/getbyuser", model, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<GetPaymentStatusDto>> GetPaymentStatusAsync(string paymentId)
        {
            return await httpClientService.GetAsync<GetPaymentStatusDto>($"{BaseUrl}/{ApiUrl}/orderdotin/paymentstatus/{paymentId}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> VerifyRazorpayOrderPaymentAsync(VerifyRazorpayPaymentDto model)
        {
            return await httpClientService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/orderdotin/verify-razorpay-order-payment", model, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DownloadDocumentDto>> GetDocumentDetailAsync(int OrderDetailId)
        {
            return await httpClientService.GetAsync<DownloadDocumentDto>($"{BaseUrl}/{ApiUrl}/orderdotin/document/{OrderDetailId}", true).ConfigureAwait(false);
        }
    }
}
