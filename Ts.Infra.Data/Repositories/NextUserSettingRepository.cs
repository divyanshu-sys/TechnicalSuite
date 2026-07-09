using Microsoft.EntityFrameworkCore;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class NextUserSettingRepository : GenericRepository<NextUserSetting>, INextUserSettingRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public NextUserSettingRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<NextUserSetting> GetWithLockFirstOrDefaultAsync()
        {
            return dbContext.NextUserSettings.FromSqlRaw("select top 1 * from NextUserSettings with(rowlock, updlock, holdlock)").SingleOrDefaultAsync();
        }
    }
}
