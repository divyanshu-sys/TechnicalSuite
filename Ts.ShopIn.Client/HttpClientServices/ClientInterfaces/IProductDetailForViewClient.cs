using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IProductDetailForViewClient
    {
        Task<ResponseMessageDto<IEnumerable<GetProductDetailForListViewVm>>> GetForListViewAsync(ProductDetailDataTableForViewRequestDto model);
        Task<ResponseMessageDto<ProductDetailVm>> GetForViewCacheAsync(string shopCategoryName, string productdetailLink);
        Task<ResponseMessageDto<bool>> UpdateProductDetailForViewCountAsync(int productdetailId, bool isBrowser);
        Task<ResponseMessageDto<string>> GetSitemapAsync();
    }
}
