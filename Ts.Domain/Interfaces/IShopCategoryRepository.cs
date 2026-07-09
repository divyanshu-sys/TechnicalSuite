using Ts.Domain.DataTableModels.ShopCategoryDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface IShopCategoryRepository : IGenericRepository<ShopCategory>
    {
        Task<List<ShopCategory>> GetAllAsync(ShopCategoryDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(ShopCategoryDataTableRequest requestModel);
    }
}
