using Ts.ShopIn.Dto.HomeDtos;

namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IHomeService
    {
        Task<DisplayHomeItemsDto> DisplayItemsAsync();
    }
}
