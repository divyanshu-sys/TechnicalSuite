using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailDocumentVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailImageVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IProductDetailClient
    {
        Task<ResponseMessageDto<ProductDetailVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<UpdateProductDetailVm>> GetForUpdateProductDetailAsync(int id);
        Task<ResponseMessageDto<UpdateProductDetailDescriptionVm>> GetForUpdateDescriptionAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<ProductDetailVm>>> GetAllAsync(ProductDetailDataTableRequestVm model, bool isShowAll = false, bool IsPagesVisited = false);
        Task<ResponseMessageDto<ProductDetailVm>> PostAsync(CreateProductDetailVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateProductDetailVm model, int productdetailId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<DownloadDocumentDto>> GetDocumentDetailAsync(int id);
        Task<ResponseMessageDto<bool>> PublishProductDetailAsync(int productdetailId, bool isRepublish);
        Task<ResponseMessageDto<bool>> UpdateDescriptionAsync(UpdateProductDetailDescriptionVm model, int productdetailId);
        Task<ResponseMessageDto<UpdateProductDetailMainImageVm>> GetForUpdateMainImageAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateProductDetailMainImageVm model, int productdetailId);
        Task<ResponseMessageDto<bool>> UpdateProductDetailImageAsync(UpdateProductDetailImageVm model, int productdetailId);
        Task<ResponseMessageDto<bool>> DeleteProductDetailImageAsync(int id, string imageName);
        Task<ResponseMessageDto<bool>> UpdateProductDetailDocumentAsync(UpdateProductDetailDocumentVm model, int productdetailId);
        Task<ResponseMessageDto<bool>> DeleteProductDetailDocumentAsync(int id);
        Task<ResponseMessageDto<UpdateProductDetailWorkerVm>> GetForUpdateProductDetailWorkerAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateProductDetailWorkerAsync(UpdateProductDetailWorkerVm model, int productdetailId);
        Task<ResponseMessageDto<int>> GetTotalUniquePagesVisited(string lastViewedOnStart = null, string lastViewedOnEnd = null);
        Task<ResponseMessageDto<int>> GetTotalPagesVisitedLifetime();
    }
}
