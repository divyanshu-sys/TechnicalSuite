using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.SubCategoryVms;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface ISubCategoryClient
    {
        Task<ResponseMessageDto<SubCategoryVm>> GetAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<SubCategoryVm>>> GetAllAsync(SubCategoryDataTableRequestVm model);
        Task<ResponseMessageDto<SubCategoryVm>> PostAsync(CreateSubCategoryVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateSubCategoryVm model, int subCategoryId);
        Task<ResponseMessageDto<IEnumerable<SubCategoryVm>>> GetAllAsync();
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync();
        Task<ResponseMessageDto<UpdateSubCategoryVm>> GetForEditAsync(int id);
    }
}
