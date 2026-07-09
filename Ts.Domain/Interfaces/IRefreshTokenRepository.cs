using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByUserIdAsync(string userId);
    }
}
