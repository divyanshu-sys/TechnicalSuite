using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class LoginLogRepository : GenericRepository<LoginLog>, ILoginLogRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public LoginLogRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
