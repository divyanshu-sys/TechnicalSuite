namespace Ts.Dto.RefreshTokenDtos
{
    public class AuthRefreshEncodeDto
    {
        public string RefreshId { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
