using Ts.Dto;
using Ts.ShopIn.Dto.HomeDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IOrderClient
    {
        Task<ResponseMessageDto<decimal>> GetTotalRevenue(string startDate = null, string endDate = null);
        Task<ResponseMessageDto<ChartResponseDto>> GetSalesData(string startDate = null, string endDate = null);
    }
}
