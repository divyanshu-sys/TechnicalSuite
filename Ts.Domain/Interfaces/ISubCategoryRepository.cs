using Ts.Domain.DataTableModels.SubCategoryDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface ISubCategoryRepository : IGenericRepository<SubCategory>
    {
        Task<List<SubCategory>> GetAllAsync(SubCategoryDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(SubCategoryDataTableRequest requestModel);
    }
}
