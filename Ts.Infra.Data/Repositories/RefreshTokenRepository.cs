using Microsoft.EntityFrameworkCore;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public RefreshTokenRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<RefreshToken> GetByUserIdAsync(string userId)
        {
            return dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
