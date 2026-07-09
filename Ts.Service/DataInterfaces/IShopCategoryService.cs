using Ts.Dto;
using Ts.Dto.DataTableDtos.ShopCategoryDataTableDtos;
using Ts.Dto.ShopCategoryDtos;
namespace Ts.Service.DataInterfaces
{
    public interface IShopCategoryService
    {
        Task<ResponseMessageDto<ShopCategoryDto>> GetAsync(int id);

        Task<DataTableResponseDto<ShopCategoryDto>> GetAllAsync(ShopCategoryDataTableRequestDto modelDto);

        Task<ResponseMessageDto<ShopCategoryDto>> CreateAsync(CreateShopCategoryDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> UpdateAsync(int shopCategoryId, UpdateShopCategoryDto modelDto, string userId);

        Task<IEnumerable<ShopCategoryDto>> GetAllAsync();
    }
}
