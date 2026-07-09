using Ts.Domain.DataTableModels.StateDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface IStateRepository : IGenericRepository<State>
    {
        Task<List<State>> GetAllAsync(StateDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(StateDataTableRequest requestModel);

        Task<List<State>> GetAllByCountryIdAsync(int countryId);
    }
}
