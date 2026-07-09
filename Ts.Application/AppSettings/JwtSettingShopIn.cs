using Microsoft.IdentityModel.Tokens;
namespace Ts.Application.AppSettings
{
    public class JwtSettingShopIn
    {
        public string Issuer { get; set; }
        public string[] Audience { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public int TokenExpireTimeInMinute { get; set; }
        public int RefreshTokenExpireTimeInMinute { get; set; }
    }
}
