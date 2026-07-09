using Ts.Dto;
using Ts.ShopIn.Client.ViewModels.HomeVms;

namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IHomeClient
    {
        Task<ResponseMessageDto<DisplayHomeItemsVm>> DisplayItemsCacheAsync();
    }
}
