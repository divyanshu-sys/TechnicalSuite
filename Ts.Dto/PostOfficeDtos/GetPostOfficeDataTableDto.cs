using Ts.Dto.DistrictDtos;
namespace Ts.Dto.PostOfficeDtos
{
    public class GetPostOfficeDataTableDto : PostOfficeDto
    {
        public GetDistrictDataTableDto District { get; set; }
    }
}
