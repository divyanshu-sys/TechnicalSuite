using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shyjus.BrowserDetection;
using Ts.Common.HelperExtensions;
namespace Ts.Application.Client.SecurityHandlers
{
    public class RazorValidateBrowserUiHandler : AuthorizationHandler<ValidateBrowserUiRequirement>
    {
        private readonly IBrowserDetector browserDetector;

        public RazorValidateBrowserUiHandler(IBrowserDetector browserDetector)
        {
            this.browserDetector = browserDetector;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidateBrowserUiRequirement requirement)
        {
            AuthorizationFilterContext authFilterContext = context.Resource as AuthorizationFilterContext;
            if (authFilterContext == null || !context.User.Identity.IsAuthenticated)
                return Task.CompletedTask;

            var deviceTypeClaim = context.User.Claims.GetBrowserDeviceType();
            var browserNameClaim = context.User.Claims.GetBrowserName();
            var browserOSClaim = context.User.Claims.GetBrowserOS();

            var descriptor = authFilterContext?.ActionDescriptor as PageActionDescriptor;

            if (deviceTypeClaim == browserDetector.Browser.DeviceType &&
                browserNameClaim == browserDetector.Browser.Name && browserOSClaim == browserDetector.Browser.OS
                || descriptor.DisplayName.Equals("/accessdenied", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
