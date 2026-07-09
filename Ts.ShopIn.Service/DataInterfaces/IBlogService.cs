using Ts.Dto;
using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.BlogImageDtos;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IBlogService
    {
        Task<ResponseMessageDto<GetUpdateBlogDto>> GetForEditAsync(int id, string userId, bool isAdmin);
        Task<DataTableResponseDto<GetBlogDataTableDto>> GetAllAsync(BlogDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null);
        Task<ResponseMessageDto<BlogDto>> CreateAsync(CreateBlogDto modelDto, string userId);
        Task<ResponseMessageDto<bool>> UpdateAsync(int blogId, UpdateBlogDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int blogId, UpdateBlogDescriptionDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int blogId, UpdateBlogMainImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> PublishBlogAsync(int blogId, PublishBlogDto modeldto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteBlogImageAsync(int id, string imageName, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateBlogImageAsync(int blogId, UpdateBlogImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateBlogWorkerAsync(int blogId, UpdateBlogWorkerDto modelDto, string userId, bool isAdmin);
        Task<IEnumerable<GetBlogForListViewDto>> GetForListViewAsync(BlogDataTableForViewRequestDto modelDto);
        Task<ResponseMessageDto<BlogDto>> GetForViewAsync(string subCategoryName, string blogLink);
        Task<string> GetSitemapAsync();

        Task IncrementBlogView2ForBlogIdsAsync(Dictionary<int, int> data);

        Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null);
        Task<int> GetTotalPagesVisitedLifetimeAsync();
    }
}
