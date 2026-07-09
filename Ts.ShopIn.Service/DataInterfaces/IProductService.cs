using Ts.Dto;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
using Ts.ShopIn.Dto.ProductDtos;
using Ts.ShopIn.Dto.ProductImageDtos;

namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IProductService
    {
        Task<ResponseMessageDto<GetUpdateProductDto>> GetForEditAsync(int id, string userId, bool isAdmin);
        Task<DataTableResponseDto<GetProductDataTableDto>> GetAllAsync(ProductDataTableRequestDto modelDto, string userId = null, bool? isAdmin = null);
        Task<ResponseMessageDto<ProductDto>> CreateAsync(CreateProductDto modelDto, string userId);
        Task<ResponseMessageDto<bool>> UpdateAsync(int productId, UpdateProductDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateMainImageAsync(int productId, UpdateProductMainImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> PublishProductAsync(int productId, PublishProductDto modeldto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> DeleteProductImageAsync(int id, string imageName, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateProductImageAsync(int productId, UpdateProductImageDto modelDto, string userId, bool isAdmin);
        Task<ResponseMessageDto<bool>> UpdateProductWorkerAsync(int productId, UpdateProductWorkerDto modelDto, string userId, bool isAdmin);
        Task<IEnumerable<GetProductForListViewDto>> GetForListViewAsync(ProductDataTableForViewRequestDto modelDto);
        Task<IEnumerable<DropdownItemDto>> GetAllForDropDownAsync();
    }
}
