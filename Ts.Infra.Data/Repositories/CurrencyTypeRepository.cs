using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class CurrencyTypeRepository : GenericRepository<CurrencyType>, ICurrencyTypeRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public CurrencyTypeRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
