using Ts.ShopIn.Domain.HelperModels;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        Task<OrderDetail> GetLatestFirstByProductDetailIdAsync(int productDetailId, int orderStatusId);
        Task<OrderDetail> GetByAsync(int id, string userId, params int[] orderStatusIds);
        Task<decimal> GetTotalPriceAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, params int[] orderStatusIds);
        Task<List<TotalPriceByMonthsHelperModel>> GetTotalPriceByMonthsAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, params int[] orderStatusIds);
    }
}
