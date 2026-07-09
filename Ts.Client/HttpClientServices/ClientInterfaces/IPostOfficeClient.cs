using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.PostOfficeVms;
using Ts.Dto;
namespace Ts.Client.HttpClientServices.ClientInterfaces
{
    public interface IPostOfficeClient
    {
        Task<ResponseMessageDto<PostOfficeVm>> GetAsync(int id);
        Task<ResponseMessageDto<UpdatePostOfficeVm>> GetForEditAsync(int id);
        Task<ResponseMessageDto<DataTableResponseVm<PostOfficeVm>>> GetAllAsync(PostOfficeDataTableRequestVm model);
        Task<ResponseMessageDto<PostOfficeVm>> PostAsync(CreatePostOfficeVm model);
        Task<ResponseMessageDto<bool>> PutAsync(UpdatePostOfficeVm model, int postOfficeId);
        Task<ResponseMessageDto<bool>> DeleteAsync(int id);
        Task<ResponseMessageDto<IEnumerable<PostOfficeVm>>> GetAllByDistrictIdAsync(int districtId);
        Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownByDistrictIdAsync(int districtId);

    }
}
