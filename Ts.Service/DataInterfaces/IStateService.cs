using Ts.Dto;
using Ts.Dto.DataTableDtos.StateDataTableDtos;
using Ts.Dto.StateDtos;
namespace Ts.Service.DataInterfaces
{
    public interface IStateService
    {
        Task<ResponseMessageDto<StateDto>> GetAsync(int id);

        Task<DataTableResponseDto<GetStateDataTableDto>> GetAllAsync(StateDataTableRequestDto modelDto);

        Task<ResponseMessageDto<StateDto>> CreateAsync(CreateStateDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> UpdateAsync(int stateId, UpdateStateDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId);

        Task<IEnumerable<StateDto>> GetAllByCountryIdAsync(int countryId);
    }
}
