using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.CartVms;
using Ts.ShopIn.Dto.CartDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface ICartClient
    {
        Task<ResponseMessageDto<CartVm>> PostAsync(CreateCartVm model);
        Task<ResponseMessageDto<GetCartForListViewDto>> GetByUserAsync();
        Task<ResponseMessageDto<bool>> DeleteAsync(int productDetailId);
    }
}
