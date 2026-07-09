using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class PaymentGatewayTypeRepository : GenericRepository<PaymentGatewayType>, IPaymentGatewayTypeRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public PaymentGatewayTypeRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
