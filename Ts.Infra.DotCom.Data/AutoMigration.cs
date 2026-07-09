using Microsoft.EntityFrameworkCore;
using Ts.Infra.DotCom.Data.Context;
namespace Ts.Infra.DotCom.Data
{
    public class AutoMigration
    {
        private readonly DotComDbContext dbContext;

        public AutoMigration(DotComDbContext dbContext)
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
