using Ts.Dto;
using Ts.Dto.DataTableDtos.DistrictDataTableDtos;
using Ts.Dto.DistrictDtos;
namespace Ts.Service.DataInterfaces
{
    public interface IDistrictService
    {
        Task<ResponseMessageDto<DistrictDto>> GetAsync(int id);

        Task<DataTableResponseDto<GetDistrictDataTableDto>> GetAllAsync(DistrictDataTableRequestDto modelDto);

        Task<ResponseMessageDto<DistrictDto>> CreateAsync(CreateDistrictDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> UpdateAsync(int districtId, UpdateDistrictDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId);

        Task<ResponseMessageDto<GetUpdateDistrictDto>> GetForEditAsync(int id);

        Task<IEnumerable<DistrictDto>> GetAllByStateIdAsync(int stateId);
    }
}
