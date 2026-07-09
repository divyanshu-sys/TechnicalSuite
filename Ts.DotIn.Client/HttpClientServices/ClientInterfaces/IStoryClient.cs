using Ts.Client.ViewModels.DataTableVms;
using Ts.DotIn.Client.ViewModels.DataTableVms;
using Ts.DotIn.Client.ViewModels.StoryImageVms;
using Ts.DotIn.Client.ViewModels.StoryRelativeVms;
using Ts.DotIn.Client.ViewModels.StoryVms;
using Ts.Dto;
namespace Ts.DotIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IStoryClient
    {
        Task<ResponseMessageDto<StoryVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<UpdateStoryVm>> GetForUpdateStoryAsync(int id);
        UpdateStoryDescriptionVm GetForUpdateDescription(StoryVm vm, string descriptionId);
        Task<ResponseMessageDto<DataTableResponseVm<StoryVm>>> GetAllAsync(StoryDataTableRequestVm model, bool isShowAll = false);
        Task<ResponseMessageDto<StoryVm>> PostAsync(CreateStoryVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateStoryVm model, int storyId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<bool>> PublishStoryAsync(int storyId, bool isRepublish);
        Task<ResponseMessageDto<bool>> AddDescriptionAsync(UpdateStoryDescriptionVm model, int storyId);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdateStoryDescriptionVm model, int storyId, string descriptionId);
        Task<ResponseMessageDto<bool>> DeleteDescriptionAsync(int storyId, string descriptionId);
        Task<ResponseMessageDto<UpdateStoryMainImageVm>> GetForUpdateMainImageAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateStoryMainImageVm model, int storyId);
        Task<ResponseMessageDto<bool>> UpdateStoryImageAsync(UpdateStoryImageVm model, int storyId);
        Task<ResponseMessageDto<bool>> DeleteStoryImageAsync(int id, string imageName);
        Task<ResponseMessageDto<UpdateStoryWorkerVm>> GetForUpdateStoryWorkerAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateStoryWorkerAsync(UpdateStoryWorkerVm model, int storyId);
        Task<ResponseMessageDto<bool>> UpdateStoryRelativeAsync(UpdateStoryRelativeVm model, int storyId);
        Task<ResponseMessageDto<bool>> DeleteStoryRelativeAsync(int id, string hrefLang);
    }
}
