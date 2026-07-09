using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.ProductVms;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IProductForViewClient
    {
        Task<ResponseMessageDto<IEnumerable<GetProductForListViewVm>>> GetForListViewAsync(ProductDataTableForViewRequestDto model);
    }
}
