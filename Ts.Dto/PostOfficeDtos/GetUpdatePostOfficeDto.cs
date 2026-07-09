using Ts.Dto.DistrictDtos;
namespace Ts.Dto.PostOfficeDtos
{
    public class GetUpdatePostOfficeDto : PostOfficeDto
    {
        public GetUpdateDistrictDto District { get; set; }
    }
}
