using Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotCom.Dto.StoryDtos;
using Ts.DotCom.Dto.StoryImageDtos;
using Ts.DotCom.Dto.StoryRelativeDtos;
using Ts.Dto;
namespace Ts.DotCom.Service.DataInterfaces
{
    public interface IStoryService
    {
        Task<ResponseMessageDto<GetUpdateStoryDto>> GetForEditAsync(int id, string userId, bool isAdmin);
        Task<DataTableResponseDto<GetStoryDataTableDto>> GetAllAsync(StoryDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null);
        Task<ResponseMessageDto<StoryDto>> CreateAsync(CreateStoryDto modelDto, string userId);
        Task<ResponseMessageDto<bool>> UpdateAsync(int storyId, UpdateStoryDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> AddDescriptionAsync(int storyId, UpdateStoryDescriptionDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int storyId, string descriptionId, UpdateStoryDescriptionDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteDescriptionAsync(int storyId, string descriptionId, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int storyId, UpdateStoryMainImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> PublishStoryAsync(int storyId, PublishStoryDto modeldto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteStoryImageAsync(int id, string imageName, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateStoryImageAsync(int storyId, UpdateStoryImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateStoryWorkerAsync(int storyId, UpdateStoryWorkerDto modelDto, string userId, bool isAdmin);
        Task<IEnumerable<GetStoryForListViewDto>> GetForListViewAsync(StoryDataTableForViewRequestDto modelDto);
        Task<ResponseMessageDto<GetStoryForViewDto>> GetForViewAsync(string subCategoryName, string storyLink);
        Task<string> GetSitemapAsync();

        [Obsolete($"This method is deprecated. Use {nameof(IncrementStoryView2ForStoryIdsAsync)} instead.")]
        Task IncrementStoryViewForStoryIdsAsync(IEnumerable<int> storyIds);

        Task IncrementStoryView2ForStoryIdsAsync(Dictionary<int, int> data);

        Task<ResponseMessageDto<bool>> UpdateStoryRelativeAsync(int storyId, UpdateStoryRelativeDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteStoryRelativeAsync(int id, string hrefLang, string userId, bool isAdmin);
    }
}
