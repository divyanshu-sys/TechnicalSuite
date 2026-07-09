using Microsoft.AspNetCore.Authorization;
using Ts.Common.HelperExtensions;
using Ts.Service.DataInterfaces;
namespace Ts.Service.ApiSecurity
{
    public class ValidateReLoginApiHandler : AuthorizationHandler<ValidateReLoginApiRequirement>
    {
        private readonly IRefreshTokenService refreshTokenService;

        public ValidateReLoginApiHandler(IRefreshTokenService refreshTokenService)
        {
            this.refreshTokenService = refreshTokenService;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidateReLoginApiRequirement requirement)
        {
            var userId = context.User.Claims.GetUserId();
            var refreshReloginId = context.User.Claims.GetRefreshReloginId();
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(refreshReloginId) && await refreshTokenService.ValidateReloginAsync(userId, refreshReloginId).ConfigureAwait(false))
                context.Succeed(requirement);
        }
    }
}
