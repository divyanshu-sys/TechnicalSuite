using Ts.DotCom.Domain.DataTableModels.PostDataTables;
using Ts.DotCom.Domain.Models;
namespace Ts.DotCom.Domain.Interfaces
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllAsync(PostDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(PostDataTableRequest requestModel);

        Task<List<Post>> GetForListViewAsync(PostDataTableForViewRequest requestModel);

        Task<Post> GetForViewAsync(int categoryId, int subCategoryId, string postLink);

        Task<List<PostView>> GetPostViewWithLockByPostIdsAsync(IEnumerable<int> postIds);

        void AddPostView(PostView entity);

        Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null);

        Task<int> GetTotalPagesVisitedLifetimeAsync();
    }
}
