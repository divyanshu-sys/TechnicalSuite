using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.DistrictVms;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface IDistrictClient
    {
        Task<ResponseMessageDto<DistrictVm>> GetAsync(int id);
        Task<ResponseMessageDto<UpdateDistrictVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<DistrictVm>>> GetAllAsync(DistrictDataTableRequestVm model);
        Task<ResponseMessageDto<DistrictVm>> PostAsync(CreateDistrictVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateDistrictVm model, int districtId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<IEnumerable<DistrictVm>>> GetAllByStateIdAsync(int stateId);
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownByStateIdAsync(int stateId);

    }
}
