namespace Ts.Dto.ApplicationUserDtos
{
    public class AuthTokenDto
    {
        public string ApiToken { get; set; }
        public string RefreshEncodedToken { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}
