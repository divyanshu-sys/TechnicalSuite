using Ts.Dto;
using Ts.Dto.CountryDtos;
using Ts.Dto.DataTableDtos.CountryDataTableDtos;
namespace Ts.Service.DataInterfaces
{
    public interface ICountryService
    {
        Task<ResponseMessageDto<CountryDto>> GetAsync(int id);

        Task<DataTableResponseDto<CountryDto>> GetAllAsync(CountryDataTableRequestDto modelDto);

        Task<ResponseMessageDto<CountryDto>> CreateAsync(CreateCountryDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> UpdateAsync(int countryId, UpdateCountryDto modelDto, string userId);

        Task<ResponseMessageDto<bool>> DeleteAsync(int id, string userId);

        Task<IEnumerable<CountryDto>> GetAllAsync();
    }
}
