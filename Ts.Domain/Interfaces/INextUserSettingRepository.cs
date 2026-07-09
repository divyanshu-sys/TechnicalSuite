using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface INextUserSettingRepository : IGenericRepository<NextUserSetting>
    {
        Task<NextUserSetting> GetWithLockFirstOrDefaultAsync();
    }
}
