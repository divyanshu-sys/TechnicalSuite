using Ts.ShopIn.Domain.DataTableModels.OrderDataTables;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetForListViewAsync(OrderDataTableForViewRequest requestModel, string userId, int orderStatusIdFilterOut);
        Task<List<Order>> GetAllAsync(OrderDataTableRequest requestModel);
        Task<int> GetRecordsFilteredAsync(OrderDataTableRequest requestModel);
    }
}
