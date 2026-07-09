using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface INextOrderSettingRepository : IGenericRepository<NextOrderSetting>
    {
        Task<NextOrderSetting> GetWithLockFirstOrDefaultAsync();
    }
}
