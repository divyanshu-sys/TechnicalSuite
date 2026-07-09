using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface INextUserSettingRepository : IGenericRepository<NextUserSetting>
    {
        Task<NextUserSetting> GetWithLockFirstOrDefaultAsync();
    }
}
