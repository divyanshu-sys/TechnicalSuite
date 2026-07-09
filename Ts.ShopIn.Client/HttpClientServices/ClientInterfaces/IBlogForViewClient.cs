using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.BlogVms;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IBlogForViewClient
    {
        Task<ResponseMessageDto<IEnumerable<GetBlogForListViewVm>>> GetForListViewAsync(BlogDataTableForViewRequestDto model);
        Task<ResponseMessageDto<BlogVm>> GetForViewCacheAsync(string subCategoryName, string blogLink);
        Task<ResponseMessageDto<bool>> UpdateBlogForViewCountAsync(int blogId, bool isBrowser);
        Task<ResponseMessageDto<string>> GetSitemapAsync();
    }
}
