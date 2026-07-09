using Microsoft.EntityFrameworkCore;
using Ts.Infra.DotIn.Data.Context;
namespace Ts.Infra.DotIn.Data
{
    public class AutoMigration
    {
        private readonly DotInDbContext dbContext;

        public AutoMigration(DotInDbContext dbContext)
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
