using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Ts.Application.AppConstants;
using Ts.Application.Helpers;
namespace Ts.Application.SecurityHandlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string Unauthorized = "Unauthorized";
        private const string AuthorizationKey = "Authorization";
        private const string AuthorizationType = "Basic";
        private readonly IConfiguration config;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IConfiguration config) : base(options, logger, encoder)
        {
            this.config = config;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationKey))
                return Task.FromResult(AuthenticateResult.Fail(Unauthorized));

            string authValue = Request.Headers[AuthorizationKey];
            if (string.IsNullOrEmpty(authValue)
                || !authValue.StartsWith($"{AuthorizationType} ", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail(Unauthorized));

            var token = authValue.Substring(6);
            try
            {
                var decodedTokenString = AesEndeCryptor.DecryptString(EnDecryptionConstant.FileServiceKey, EnDecryptionConstant.FileServiceIv, token);

                var credential = decodedTokenString.Split(';');

                if (credential.Length != 4)
                    Task.FromResult(AuthenticateResult.Fail(Unauthorized));

                if (Convert.ToDateTime(credential[3]) < DateTime.UtcNow)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Token has expired."));
                }

                if (credential[0] != config.GetValue<string>("Auth:Username") && credential[1] != config.GetValue<string>("Auth:Password"))
                    Task.FromResult(AuthenticateResult.Fail(Unauthorized));

                var claims = new[] { new Claim(ClaimTypes.Name, credential[0]), new Claim(ClaimTypes.Role, credential[2]) };
                var identity = new ClaimsIdentity(claims, AuthorizationType);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid refresh token supplied."));
            }
        }
    }
}
