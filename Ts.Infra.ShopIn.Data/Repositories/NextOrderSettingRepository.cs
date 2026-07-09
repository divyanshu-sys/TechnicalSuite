using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class NextOrderSettingRepository : GenericRepository<NextOrderSetting>, INextOrderSettingRepository
    {
        private readonly ShopInDbContext dbContext;

        public NextOrderSettingRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<NextOrderSetting> GetWithLockFirstOrDefaultAsync()
        {
            return dbContext.NextOrderSettings.FromSqlRaw("select top 1 * from NextOrderSettings with(rowlock, updlock, holdlock)").SingleOrDefaultAsync();
        }
    }
}
