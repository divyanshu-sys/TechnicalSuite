using Ts.ShopIn.Domain.DataTableModels.BlogDataTables;
using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Task<List<Blog>> GetAllAsync(BlogDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(BlogDataTableRequest requestModel);

        Task<List<Blog>> GetForListViewAsync(BlogDataTableForViewRequest requestModel);

        Task<Blog> GetForViewAsync(int subCategoryId, string blogLink);

        Task<List<BlogView>> GetBlogViewWithLockByBlogIdsAsync(IEnumerable<int> blogIds);

        void AddBlogView(BlogView entity);

        Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null);

        Task<int> GetTotalPagesVisitedLifetimeAsync();
    }
}
