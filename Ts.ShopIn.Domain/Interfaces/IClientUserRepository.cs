using Ts.ShopIn.Domain.DataTableModels.ClientUserDataTables;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IClientUserRepository
    {
        Task<List<ClientUser>> GetAllAsync(ClientUserDataTableRequest requestModel);
        Task<int> GetRecordsFilteredAsync(ClientUserDataTableRequest requestModel);
        Task<int> GetTotalCountAsync();
        Task<List<ClientUser>> GetUsersByRolesAsync(IEnumerable<string> roles);
        Task<ClientUser> GetForCartOrderAsync(string id);
    }
}
