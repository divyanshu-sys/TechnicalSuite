using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.OrderVms;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;
using Ts.ShopIn.Dto.OrderDtos;
using Ts.ShopIn.Dto.PaymentDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IOrderForViewClient
    {
        Task<ResponseMessageDto<GetRazorpayOrderDto>> CreateRazorpayOrderAsync();
        Task<ResponseMessageDto<bool>> VerifyRazorpayOrderPaymentAsync(VerifyRazorpayPaymentDto model);
        Task<ResponseMessageDto<GetPaymentStatusDto>> GetPaymentStatusAsync(string paymentId);
        Task<ResponseMessageDto<IEnumerable<OrderVm>>> GetByUserAsync(OrderDataTableForViewRequestDto model);
        Task<ResponseMessageDto<DownloadDocumentDto>> GetDocumentDetailAsync(int OrderDetailId);
    }
}
