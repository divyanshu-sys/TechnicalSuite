using Ts.Dto.CountryDtos;
namespace Ts.Dto.StateDtos
{
    public class GetProfileStateDto : StateDto
    {
        public CountryDto Country { get; set; }
    }
}
