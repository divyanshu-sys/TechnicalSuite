using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class PaymentStatusRepository : GenericRepository<PaymentStatus>, IPaymentStatusRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public PaymentStatusRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
