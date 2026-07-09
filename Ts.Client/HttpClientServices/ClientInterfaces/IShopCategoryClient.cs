using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.ShopCategoryVms;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface IShopCategoryClient
    {
        Task<ResponseMessageDto<ShopCategoryVm>> GetAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<ShopCategoryVm>>> GetAllAsync(ShopCategoryDataTableRequestVm model);
        Task<ResponseMessageDto<ShopCategoryVm>> PostAsync(CreateShopCategoryVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateShopCategoryVm model, int shopCategoryId);
        Task<ResponseMessageDto<IEnumerable<ShopCategoryVm>>> GetAllAsync();
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync();
        Task<ResponseMessageDto<UpdateShopCategoryVm>> GetForEditAsync(int id);
    }
}
