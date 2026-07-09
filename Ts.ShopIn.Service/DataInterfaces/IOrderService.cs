using Ts.Dto;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;
using Ts.ShopIn.Dto.HomeDtos;
using Ts.ShopIn.Dto.OrderDtos;
using Ts.ShopIn.Dto.PaymentDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;

namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IOrderService
    {
        Task<ResponseMessageDto<GetRazorpayOrderDto>> CreateRazorpayOrderAsync(string userId);
        Task<ResponseMessageDto<bool>> VerifyRazorpayOrderPaymentAsync(VerifyRazorpayPaymentDto modelDto, string userId);
        Task<ResponseMessageDto<GetPaymentStatusDto>> GetRazorpayPaymentStatusAsync(string razorpayPaymentId, string userId);
        Task<IEnumerable<GetOrderForViewDto>> GetForListViewAsync(OrderDataTableForViewRequestDto modelDto, string userId);
        Task<ResponseMessageDto<DownloadDocumentDto>> GetProductDocumentDetailAsync(int orderDetailId, string userId);
        Task<ResponseMessageDto<bool>> ProcessRazorpayWebhookAsync(string requestBody, string receivedSignature);
        Task<decimal> GetTotalRevenue(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null);
        Task<ChartResponseDto> GetTotalSalesDataAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null);
    }
}
