using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Client.ViewModels.OrderVms;
using Ts.ShopIn.Dto.HomeDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IOrderClient
    {
        Task<ResponseMessageDto<decimal>> GetTotalRevenue(string startDate = null, string endDate = null);
        Task<ResponseMessageDto<ChartResponseDto>> GetSalesData(string startDate = null, string endDate = null);
        Task<ResponseMessageDto<DataTableResponseVm<OrderVm>>> GetAllAsync(OrderDataTableRequestVm model);
    }
}
