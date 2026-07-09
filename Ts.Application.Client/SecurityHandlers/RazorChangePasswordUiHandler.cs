using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.Common.HelperExtensions;
namespace Ts.Application.Client.SecurityHandlers
{
    public class RazorChangePasswordUiHandler : AuthorizationHandler<ChangePasswordUiRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ChangePasswordUiRequirement requirement)
        {
            if (context.User.Claims.GetChangePassword())
            {
                var authFilterContext = context.Resource as AuthorizationFilterContext;
                var descriptor = authFilterContext?.ActionDescriptor as PageActionDescriptor;
                if (authFilterContext != null && context.User.Identity.IsAuthenticated && descriptor != null && !descriptor.DisplayName.Equals("/account/changepassword", StringComparison.OrdinalIgnoreCase))
                    authFilterContext.Result = new RedirectToPageResult("/account/changepassword");
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
