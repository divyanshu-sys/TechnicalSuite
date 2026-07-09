using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
namespace Ts.Infra.ShopIn.Data
{
    public class AutoMigration
    {
        private readonly ShopInDbContext dbContext;

        public AutoMigration(ShopInDbContext dbContext)
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
