using Ts.Domain.DataTableModels.CountryDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<List<Country>> GetAllAsync(CountryDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(CountryDataTableRequest requestModel);
    }
}
