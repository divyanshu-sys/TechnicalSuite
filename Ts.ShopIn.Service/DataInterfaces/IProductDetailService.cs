using Ts.Dto;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;
using Ts.ShopIn.Dto.ProductDetailImageDtos;
namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IProductDetailService
    {
        Task<ResponseMessageDto<GetUpdateProductDetailDto>> GetForEditAsync(int id, string userId, bool isAdmin);
        Task<DataTableResponseDto<GetProductDetailDataTableDto>> GetAllAsync(ProductDetailDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null);
        Task<ResponseMessageDto<ProductDetailDto>> CreateAsync(CreateProductDetailDto modelDto, string userId);
        Task<ResponseMessageDto<bool>> UpdateAsync(int productdetailId, UpdateProductDetailDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(int productdetailId, UpdateProductDetailDescriptionDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int productdetailId, UpdateProductDetailMainImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> PublishProductDetailAsync(int productdetailId, PublishProductDetailDto modeldto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteProductDetailImageAsync(int id, string imageName, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateProductDetailImageAsync(int productdetailId, UpdateProductDetailImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteProductDetailDocumentAsync(int id, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateProductDetailDocumentAsync(int productdetailId, UpdateProductDetailDocumentDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateProductDetailWorkerAsync(int productdetailId, UpdateProductDetailWorkerDto modelDto, string userId, bool isAdmin);
        Task<IEnumerable<GetProductDetailForListViewDto>> GetForListViewAsync(ProductDetailDataTableForViewRequestDto modelDto);
        Task<ResponseMessageDto<GetForViewProductDetailDto>> GetForViewAsync(string subCategoryName, string productdetailLink);
        Task<ResponseMessageDto<DownloadDocumentDto>> GetProductDocumentDetailAsync(int id, string userId, bool isAdmin);
        Task<string> GetSitemapAsync();

        Task IncrementProductDetailView2ForProductDetailIdsAsync(Dictionary<int, int> data);

        Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null);
        Task<int> GetTotalPagesVisitedLifetimeAsync();
    }
}
