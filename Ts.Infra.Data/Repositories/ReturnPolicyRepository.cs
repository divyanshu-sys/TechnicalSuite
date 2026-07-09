using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class ReturnPolicyRepository : GenericRepository<ReturnPolicy>, IReturnPolicyRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public ReturnPolicyRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
