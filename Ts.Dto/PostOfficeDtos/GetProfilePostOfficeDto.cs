using Ts.Dto.DistrictDtos;
namespace Ts.Dto.PostOfficeDtos
{
    public class GetProfilePostOfficeDto : PostOfficeDto
    {
        public GetProfileDistrictDto District { get; set; }
    }
}
