using Ts.DotIn.Domain.DataTableModels.StoryDataTables;
using Ts.DotIn.Domain.Models;
namespace Ts.DotIn.Domain.Interfaces
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
