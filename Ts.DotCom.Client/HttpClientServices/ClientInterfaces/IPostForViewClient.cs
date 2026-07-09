using Ts.DotCom.Client.ViewModels.PostVms;
using Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos;
using Ts.Dto;
namespace Ts.DotCom.Client.HttpClientServices.ClientInterfaces
{
    public interface IPostForViewClient
    {
        Task<ResponseMessageDto<IEnumerable<GetPostForListViewVm>>> GetForListViewAsync(PostDataTableForViewRequestDto model);
        Task<ResponseMessageDto<PostVm>> GetForViewCacheAsync(string categoryName, string subCategoryName, string postLink);
        Task<ResponseMessageDto<bool>> UpdatePostForViewCountAsync(int postId, bool isBrowser);
        Task<ResponseMessageDto<string>> GetSitemapAsync();
        Task<ResponseMessageDto<IEnumerable<GetPostForListViewVm>>> DisplayItemsCacheAsync(PostDataTableForViewRequestDto model);
    }
}
