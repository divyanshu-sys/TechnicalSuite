using Ts.Dto.AddressDtos;
namespace Ts.Dto.ApplicationUserDtos
{
    public class GetProfileDto : ApplicationUserDto
    {
        public GetProfileAddressDto Address { get; set; }
    }
}
