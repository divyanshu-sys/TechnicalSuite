using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.DataTableModels.OrderDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ShopInDbContext dbContext;

        public OrderRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Order>> GetForListViewAsync(OrderDataTableForViewRequest requestModel, string userId, int orderStatusIdFilterOut)
        {
            return dbContext.Orders
            // 1. Primary Filter (Gatekeeper)
            .Where(x => x.UserId == userId && x.OrderDetails.Any(od => od.OrderStatusId != orderStatusIdFilterOut))

            // 2. Filtered Loading (Only load the items we want to see)
            .Include(x => x.OrderDetails.Where(od => od.OrderStatusId != orderStatusIdFilterOut))
                .ThenInclude(x => x.ProductDetail)

            // 3. Sorting and Pagination
            .OrderByDescending(x => x.OrderedOn)
            .Skip(requestModel.Start)
            .Take(requestModel.Length)

            // 4. Performance Optimization
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
        }
    }
}
