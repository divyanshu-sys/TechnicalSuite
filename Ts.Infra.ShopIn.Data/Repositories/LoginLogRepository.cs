using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class LoginLogRepository : GenericRepository<LoginLog>, ILoginLogRepository
    {
        private readonly ShopInDbContext dbContext;

        public LoginLogRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
