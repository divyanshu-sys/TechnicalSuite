using Ts.DotIn.Client.ViewModels.StoryVms;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.Dto;
namespace Ts.DotIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IStoryForViewClient
    {
        Task<ResponseMessageDto<IEnumerable<GetStoryForListViewVm>>> GetForListViewAsync(StoryDataTableForViewRequestDto model);
        Task<ResponseMessageDto<StoryVm>> GetForViewCacheAsync(string subCategoryName, string storyLink);
        Task<ResponseMessageDto<bool>> UpdateStoryForViewCountAsync(int storyId, bool isBrowser);
        Task<ResponseMessageDto<string>> GetSitemapAsync();
    }
}
