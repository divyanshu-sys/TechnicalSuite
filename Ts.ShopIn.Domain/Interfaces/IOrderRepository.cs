using Ts.ShopIn.Domain.DataTableModels.OrderDataTables;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetForListViewAsync(OrderDataTableForViewRequest requestModel, string userId, int orderStatusIdFilterOut);
    }
}
