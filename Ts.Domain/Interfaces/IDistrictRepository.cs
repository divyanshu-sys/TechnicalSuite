using Ts.Domain.DataTableModels.DistrictDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface IDistrictRepository : IGenericRepository<District>
    {
        Task<List<District>> GetAllAsync(DistrictDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(DistrictDataTableRequest requestModel);

        Task<List<District>> GetAllByStateIdAsync(int stateId);
    }
}
