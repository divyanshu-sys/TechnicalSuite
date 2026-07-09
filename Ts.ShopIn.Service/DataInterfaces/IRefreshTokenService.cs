using Ts.Dto.RefreshTokenDtos;
namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IRefreshTokenService
    {
        Task<bool> DeleteByUserIdAsync(string userId);

        Task<RefreshTokenDto> CreateByUserIdAsync(string userId);
    }
}
