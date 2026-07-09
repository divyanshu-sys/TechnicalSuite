namespace Ts.Dto.PostOfficeDtos
{
    public class PostOfficeDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pincode { get; set; }
        public int DistrictId { get; set; }
    }
}
