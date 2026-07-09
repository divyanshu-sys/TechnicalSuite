using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.BlogImageVms;
using Ts.ShopIn.Client.ViewModels.BlogVms;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IBlogClient
    {
        Task<ResponseMessageDto<BlogVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<UpdateBlogVm>> GetForUpdateBlogAsync(int id);
        Task<ResponseMessageDto<UpdateBlogDescriptionVm>> GetForUpdateDescriptionAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<BlogVm>>> GetAllAsync(BlogDataTableRequestVm model, bool isShowAll = false, bool IsPagesVisited = false);
        Task<ResponseMessageDto<BlogVm>> PostAsync(CreateBlogVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateBlogVm model, int blogId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<bool>> PublishBlogAsync(int blogId, bool isRepublish);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdateBlogDescriptionVm model, int blogId);
        Task<ResponseMessageDto<UpdateBlogMainImageVm>> GetForUpdateMainImageAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateBlogMainImageVm model, int blogId);
        Task<ResponseMessageDto<bool>> UpdateBlogImageAsync(UpdateBlogImageVm model, int blogId);
        Task<ResponseMessageDto<bool>> DeleteBlogImageAsync(int id, string imageName);
        Task<ResponseMessageDto<UpdateBlogWorkerVm>> GetForUpdateBlogWorkerAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateBlogWorkerAsync(UpdateBlogWorkerVm model, int blogId);
        Task<ResponseMessageDto<int>> GetTotalUniquePagesVisited(string lastViewedOnStart = null, string lastViewedOnEnd = null);
        Task<ResponseMessageDto<int>> GetTotalPagesVisitedLifetime();
    }
}
