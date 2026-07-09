using Ts.Domain.DataTableModels.CategoryDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetAllAsync(CategoryDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(CategoryDataTableRequest requestModel);
    }
}
