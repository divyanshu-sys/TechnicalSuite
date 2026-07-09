using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class ExchangePolicyRepository : GenericRepository<ExchangePolicy>, IExchangePolicyRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public ExchangePolicyRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
