using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class OrderDetailStatusHistoryRepository : GenericRepository<OrderDetailStatusHistory>, IOrderDetailStatusHistoryRepository
    {
        private readonly ShopInDbContext dbContext;

        public OrderDetailStatusHistoryRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
