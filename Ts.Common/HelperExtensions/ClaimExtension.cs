using System.Security.Claims;
namespace Ts.Common.HelperExtensions
{
    public static class ClaimExtension
    {
        public static string GetUserId(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "id");
            return claim?.Value;
        }

        public static string GetRefreshReloginId(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "refreshreloginid");
            return claim?.Value;
        }

        public static string GetRefreshToken(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "refreshtoken");
            return claim?.Value;
        }

        public static string GetAuthenticationScheme(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "authenticationscheme");
            return claim?.Value;
        }

        public static string GetApiToken(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "token");
            return claim?.Value;
        }

        public static bool GetChangePassword(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "changepassword");
            return string.IsNullOrEmpty(claim?.Value) ? false : Convert.ToBoolean(claim.Value);
        }

        public static bool GetIsPersistent(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "ispersistent");
            return string.IsNullOrEmpty(claim?.Value) ? false : Convert.ToBoolean(claim.Value);
        }

        public static DateTime? GetRefreshTokenExpiryAt(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "refreshexpiryat");
            return string.IsNullOrEmpty(claim?.Value) ? null : Convert.ToDateTime(claim.Value);
        }

        public static string GetBrowserDeviceType(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "devicetype");
            return claim?.Value;
        }

        public static string GetBrowserName(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "browsername");
            return claim?.Value;
        }

        public static string GetBrowserOS(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "browseros");
            return claim?.Value;
        }

        public static string GetBrowserVersion(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == "browserversion");
            return claim?.Value;
        }
    }
}
