using Ts.Dto.PostOfficeDtos;
namespace Ts.Dto.AddressDtos
{
    public class GetProfileAddressDto : AddressDto
    {
        public GetProfilePostOfficeDto PostOffice { get; set; }
    }
}
