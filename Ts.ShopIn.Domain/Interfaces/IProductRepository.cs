using Ts.ShopIn.Domain.DataTableModels.ProductDataTables;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllAsync(ProductDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(ProductDataTableRequest requestModel);

        Task<List<Product>> GetForListViewAsync(ProductDataTableForViewRequest requestModel, List<int> downloadableDeliveryPolicyIds);
    }
}
