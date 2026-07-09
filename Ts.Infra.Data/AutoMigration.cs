using Microsoft.EntityFrameworkCore;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data
{
    public class AutoMigration
    {
        private readonly TsIdentityDbContext dbContext;

        public AutoMigration(TsIdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Initialize()
        {
            if (dbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
            }
        }
    }
}
