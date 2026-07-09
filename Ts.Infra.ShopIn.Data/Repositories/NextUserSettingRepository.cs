using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class NextUserSettingRepository : GenericRepository<NextUserSetting>, INextUserSettingRepository
    {
        private readonly ShopInDbContext dbContext;

        public NextUserSettingRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<NextUserSetting> GetWithLockFirstOrDefaultAsync()
        {
            return dbContext.NextUserSettings.FromSqlRaw("select top 1 * from NextUserSettings with(rowlock, updlock, holdlock)").SingleOrDefaultAsync();
        }
    }
}
