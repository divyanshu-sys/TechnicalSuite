using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class PaymentModeRepository : GenericRepository<PaymentMode>, IPaymentModeRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public PaymentModeRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
