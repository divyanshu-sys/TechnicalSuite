using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Client.ViewModels.ProductImageVms;
using Ts.ShopIn.Client.ViewModels.ProductVms;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IProductClient
    {
        Task<ResponseMessageDto<ProductVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<UpdateProductVm>> GetForUpdateProductAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<ProductVm>>> GetAllAsync(ProductDataTableRequestVm model, bool isShowAll = false);
        Task<ResponseMessageDto<ProductVm>> PostAsync(CreateProductVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateProductVm model, int productId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<bool>> PublishProductAsync(int productId, bool isRepublish);
        Task<ResponseMessageDto<UpdateProductMainImageVm>> GetForUpdateMainImageAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(UpdateProductMainImageVm model, int productId);
        Task<ResponseMessageDto<bool>> UpdateProductImageAsync(UpdateProductImageVm model, int productId);
        Task<ResponseMessageDto<bool>> DeleteProductImageAsync(int id, string imageName);
        Task<ResponseMessageDto<UpdateProductWorkerVm>> GetForUpdateProductWorkerAsync(int id);
        Task<ResponseMessageDto<bool>> UpdateProductWorkerAsync(UpdateProductWorkerVm model, int productId);
        Task<ResponseMessageDto<IEnumerable<DropdownItemDto>>> GetAllForDropDownAsync();
    }
}
