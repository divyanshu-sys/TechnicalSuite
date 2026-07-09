namespace Ts.Dto.AddressDtos
{
    public class AddressDto : BaseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int PostOfficeId { get; set; }
    }
}
