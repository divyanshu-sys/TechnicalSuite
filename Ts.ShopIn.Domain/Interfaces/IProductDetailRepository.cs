using Ts.ShopIn.Domain.DataTableModels.ProductDetailDataTables;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IProductDetailRepository : IGenericRepository<ProductDetail>
    {
        Task<List<ProductDetail>> GetAllAsync(ProductDetailDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(ProductDetailDataTableRequest requestModel);

        Task<List<ProductDetail>> GetForListViewAsync(ProductDetailDataTableForViewRequest requestModel, List<int> downloadableDeliveryPolicyIds);

        Task<ProductDetail> GetForViewAsync(int shopCategoryId, string productdetailLink, List<int> downloadableDeliveryPolicyIds);

        Task<List<ProductDetailView>> GetProductDetailViewWithLockByProductDetailIdsAsync(IEnumerable<int> productdetailIds);

        void AddProductDetailView(ProductDetailView entity);

        Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null);

        Task<int> GetTotalPagesVisitedLifetimeAsync();
    }
}
