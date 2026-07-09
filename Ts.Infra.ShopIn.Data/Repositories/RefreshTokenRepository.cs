using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ShopInDbContext dbContext;

        public RefreshTokenRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<RefreshToken> GetByUserIdAsync(string userId)
        {
            return dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
