using Ts.Domain.DataTableModels.ApplicationUserDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<List<ApplicationUser>> GetAllAsync(ApplicationUserDataTableRequest requestModel);
        Task<int> GetRecordsFilteredAsync(ApplicationUserDataTableRequest requestModel);
        Task<int> GetTotalCountAsync();
        Task<List<ApplicationUser>> GetUsersByRolesAsync(IEnumerable<string> roles);
        Task<List<ApplicationUser>> GetUsersByUserIdsAsync(IEnumerable<string> userIds);
    }
}
