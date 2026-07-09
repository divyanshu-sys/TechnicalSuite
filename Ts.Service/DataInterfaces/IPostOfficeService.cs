using Ts.Dto;
using Ts.Dto.DataTableDtos.PostOfficeDataTableDtos;
using Ts.Dto.PostOfficeDtos;
namespace Ts.Service.DataInterfaces
{
    public interface IPostOfficeService
    {
        Task<ResponseMessageDto<PostOfficeDto>> GetAsync(int id);

        Task<DataTableResponseDto<GetPostOfficeDataTableDto>> GetAllAsync(PostOfficeDataTableRequestDto modelDto);

        Task<ResponseMessageDto<PostOfficeDto>> CreateAsync(CreatePostOfficeDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> UpdateAsync(int postOfficeId, UpdatePostOfficeDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId);

        Task<ResponseMessageDto<GetUpdatePostOfficeDto>> GetForEditAsync(int id);

        Task<IEnumerable<PostOfficeDto>> GetAllByDistrictIdAsync(int districtId);
    }
}
