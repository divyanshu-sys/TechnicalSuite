using Ts.Client.ViewModels.DataTableVms;
using Ts.DotCom.Client.ViewModels.DataTableVms;
using Ts.DotCom.Client.ViewModels.PostImageVms;
using Ts.DotCom.Client.ViewModels.PostRelativeVms;
using Ts.DotCom.Client.ViewModels.PostVms;
using Ts.Dto;
namespace Ts.DotCom.Client.HttpClientServices.ClientInterfaces
{
    public interface IPostClient
    {
        Task<ResponseMessageDto<PostVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<UpdatePostVm>> GetForUpdatePostAsync(int id);
        Task<ResponseMessageDto<UpdatePostDescriptionVm>> GetForUpdateDescriptionAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<PostVm>>> GetAllAsync(PostDataTableRequestVm model, bool isShowAll = false, bool IsPagesVisited = false);
        Task<ResponseMessageDto<PostVm>> PostAsync(CreatePostVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdatePostVm model, int postId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<bool>> PublishPostAsync(int postId, bool isRepublish);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdatePostDescriptionVm model, int postId);
        Task<ResponseMessageDto<UpdatePostMainImageVm>> GetForUpdateMainImageAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdatePostMainImageVm model, int postId);
        Task<ResponseMessageDto<bool>> UpdatePostImageAsync(UpdatePostImageVm model, int postId);
        Task<ResponseMessageDto<bool>> DeletePostImageAsync(int id, string imageName);
        Task<ResponseMessageDto<UpdatePostWorkerVm>> GetForUpdatePostWorkerAsync(int id);
        Task<ResponseMessageDto<bool>> UpdatePostWorkerAsync(UpdatePostWorkerVm model, int postId);
        Task<ResponseMessageDto<int>> GetTotalUniquePagesVisited(string lastViewedOnStart = null, string lastViewedOnEnd = null);
        Task<ResponseMessageDto<int>> GetTotalPagesVisitedLifetime();
        Task<ResponseMessageDto<bool>> UpdatePostRelativeAsync(UpdatePostRelativeVm model, int postId);
        Task<ResponseMessageDto<bool>> DeletePostRelativeAsync(int id, string hrefLang);
    }
}
