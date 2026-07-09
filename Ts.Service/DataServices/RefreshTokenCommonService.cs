using Microsoft.Extensions.Caching.Distributed;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
namespace Ts.Service.DataServices
{
    public static class RefreshTokenCommonService
    {
        public const string EntityCacheKey = "RefereshToken";
        public const string UserIdCacheKey = "UserId";

        public static async Task<bool> DeleteByUserIdAsync(string userId, IUnitOfWork unitOfWork, IDistributedCache cache)
        {
            var cacheKey = $"{EntityCacheKey}_{UserIdCacheKey}_{userId}";
            await cache.RemoveAsync(cacheKey).ConfigureAwait(false);

            var refreshToken = await unitOfWork.RefreshTokenRepo.GetByUserIdAsync(userId).ConfigureAwait(false);
            if (refreshToken == null) return true;
            unitOfWork.RefreshTokenRepo.Delete(refreshToken);
            return false;
        }

        public static RefreshToken CreateByUserId(string userId, IUnitOfWork unitOfWork)
        {
            var entity = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                RefreshReloginId = Guid.NewGuid().ToString(),
                UserId = userId
            };
            unitOfWork.RefreshTokenRepo.Create(entity);
            return entity;
        }
    }
}
