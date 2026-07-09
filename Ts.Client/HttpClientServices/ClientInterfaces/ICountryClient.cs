using Ts.Client.ViewModels.CountryVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface ICountryClient
    {
        Task<ResponseMessageDto<CountryVm>> GetAsync(int id);
        Task<ResponseMessageDto<UpdateCountryVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<CountryVm>>> GetAllAsync(CountryDataTableRequestVm model);
        Task<ResponseMessageDto<CountryVm>> PostAsync(CreateCountryVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdateCountryVm model, int countryId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<IEnumerable<CountryVm>>> GetAllAsync();
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync();
    }
}
