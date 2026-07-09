using Ts.Client.ViewModels.CategoryVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface ICategoryClient
    {
        Task<ResponseMessageDto<CategoryVm>> GetAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<CategoryVm>>> GetAllAsync(CategoryDataTableRequestVm model);
        Task<ResponseMessageDto<CategoryVm>> PostAsync(CreateCategoryVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateCategoryVm model, int categoryId);
        Task<ResponseMessageDto<IEnumerable<CategoryVm>>> GetAllAsync();
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync();
        Task<ResponseMessageDto<UpdateCategoryVm>> GetForEditAsync(int id);
    }
}
