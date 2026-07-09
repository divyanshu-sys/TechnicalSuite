using Ts.Dto;
using Ts.ShopIn.Dto.CartDtos;

namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface ICartService
    {
        Task<ResponseMessageDto<CartDto>> CreateAsync(CreateCartDto modelDto, string userId);
        Task<GetCartForListViewDto> GetByUserIdAsync(string userId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int productDetailId, string userId);
    }
}
