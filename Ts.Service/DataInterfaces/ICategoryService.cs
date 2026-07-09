using Ts.Dto;
using Ts.Dto.CategoryDtos;
using Ts.Dto.DataTableDtos.CategoryDataTableDtos;
namespace Ts.Service.DataInterfaces
{
    public interface ICategoryService
    {
        Task<ResponseMessageDto<CategoryDto>> GetAsync(int id);

        Task<DataTableResponseDto<CategoryDto>> GetAllAsync(CategoryDataTableRequestDto modelDto);

        Task<ResponseMessageDto<CategoryDto>> CreateAsync(CreateCategoryDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> UpdateAsync(int categoryId, UpdateCategoryDto modelDto, string userId);

        Task<IEnumerable<CategoryDto>> GetAllAsync();
    }
}
