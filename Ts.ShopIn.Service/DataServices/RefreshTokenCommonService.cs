using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Service.DataServices
{
    public static class RefreshTokenCommonService
    {
        public static async Task<bool> DeleteByUserIdAsync(string userId, IUnitOfWork unitOfWork)
        {
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
