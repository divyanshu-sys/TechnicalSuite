using Ts.Dto;
using Ts.Dto.DataTableDtos.SubCategoryDataTableDtos;
using Ts.Dto.SubCategoryDtos;
namespace Ts.Service.DataInterfaces
{
    public interface ISubCategoryService
    {
        Task<ResponseMessageDto<SubCategoryDto>> GetAsync(int id);

        Task<DataTableResponseDto<SubCategoryDto>> GetAllAsync(SubCategoryDataTableRequestDto modelDto);

        Task<ResponseMessageDto<SubCategoryDto>> CreateAsync(CreateSubCategoryDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> UpdateAsync(int subCategoryId, UpdateSubCategoryDto modelDto, string userId);

        Task<IEnumerable<SubCategoryDto>> GetAllAsync();
    }
}
