using Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotIn.Dto.PostDtos;
using Ts.DotIn.Dto.PostImageDtos;
using Ts.DotIn.Dto.PostRelativeDtos;
using Ts.Dto;
namespace Ts.DotIn.Service.DataInterfaces
{
    public interface IPostService
    {
        Task<ResponseMessageDto<GetUpdatePostDto>> GetForEditAsync(int id, string userId, bool isAdmin);
        Task<DataTableResponseDto<GetPostDataTableDto>> GetAllAsync(PostDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null);
        Task<ResponseMessageDto<PostDto>> CreateAsync(CreatePostDto modelDto, string userId);
        Task<ResponseMessageDto<bool>> UpdateAsync(int postId, UpdatePostDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int postId, UpdatePostDescriptionDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int postId, UpdatePostMainImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> PublishPostAsync(int postId, PublishPostDto modeldto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeletePostImageAsync(int id, string imageName, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdatePostImageAsync(int postId, UpdatePostImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdatePostWorkerAsync(int postId, UpdatePostWorkerDto modelDto, string userId, bool isAdmin);
        Task<IEnumerable<GetPostForListViewDto>> GetForListViewAsync(PostDataTableForViewRequestDto modelDto);
        Task<ResponseMessageDto<GetPostForViewDto>> GetForViewAsync(string categoryName, string subCategoryName, string postLink);
        Task<string> GetSitemapAsync();

        [Obsolete($"This method is deprecated. Use {nameof(IncrementPostView2ForPostIdsAsync)} instead.")]
        Task IncrementPostViewForPostIdsAsync(IEnumerable<int> postIds);

        Task IncrementPostView2ForPostIdsAsync(Dictionary<int, int> data);

        Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null);
        Task<int> GetTotalPagesVisitedLifetimeAsync();
        Task<ResponseMessageDto<bool>> UpdatePostRelativeAsync(int postId, UpdatePostRelativeDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeletePostRelativeAsync(int id, string hrefLang, string userId, bool isAdmin);
    }
}
