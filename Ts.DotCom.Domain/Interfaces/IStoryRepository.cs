using Ts.DotCom.Domain.DataTableModels.StoryDataTables;
using Ts.DotCom.Domain.Models;
namespace Ts.DotCom.Domain.Interfaces
{
    public interface IStoryRepository : IGenericRepository<Story>
    {
        Task<List<Story>> GetAllAsync(StoryDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(StoryDataTableRequest requestModel);

        Task<List<Story>> GetForListViewAsync(StoryDataTableForViewRequest requestModel);

        Task<Story> GetForViewAsync(int subCategoryId, string storyLink);

        Task<List<StoryView>> GetStoryViewWithLockByStoryIdsAsync(IEnumerable<int> storyIds);

        void AddStoryView(StoryView entity);
    }
}
