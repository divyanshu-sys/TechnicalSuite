using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class PaymentStatusHistoryRepository : GenericRepository<PaymentStatusHistory>, IPaymentStatusHistoryRepository
    {
        private readonly ShopInDbContext dbContext;

        public PaymentStatusHistoryRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
