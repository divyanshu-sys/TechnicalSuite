using Ts.Dto.RefreshTokenDtos;
namespace Ts.Service.DataInterfaces
{
    public interface IRefreshTokenService
    {
        Task<bool> DeleteByUserIdAsync(string userId);

        Task<bool> ValidateReloginAsync(string userId, string refreshReloginId);

        Task<RefreshTokenDto> CreateByUserIdAsync(string userId);
    }
}
