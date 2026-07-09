using Ts.Dto.PostOfficeDtos;
namespace Ts.Dto.AddressDtos
{
    public class GetUpdateAddressDto : AddressDto
    {
        public GetUpdatePostOfficeDto PostOffice { get; set; }
    }
}
