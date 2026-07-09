using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.StateVms;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface IStateClient
    {
        Task<ResponseMessageDto<StateVm>> GetAsync(int id);
        Task<ResponseMessageDto<UpdateStateVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<StateVm>>> GetAllAsync(StateDataTableRequestVm model);
        Task<ResponseMessageDto<StateVm>> PostAsync(CreateStateVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateStateVm model, int stateId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<IEnumerable<StateVm>>> GetAllByCountryIdAsync(int countryId);
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownByCountryIdAsync(int countryId);
    }
}
