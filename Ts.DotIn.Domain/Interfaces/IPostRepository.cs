using Ts.DotIn.Domain.DataTableModels.PostDataTables;
using Ts.DotIn.Domain.Models;
namespace Ts.DotIn.Domain.Interfaces
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
