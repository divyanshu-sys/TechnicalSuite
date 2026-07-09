using Ts.Dto.CountryDtos;
namespace Ts.Dto.StateDtos
{
    public class GetStateDataTableDto : StateDto
    {
        public CountryDto Country { get; set; }
    }
}
