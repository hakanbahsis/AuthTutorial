using Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Permissions;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
{
    public PermissionAuthorizationHandler(){ }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
    {
        if (context.User is null)
        {
            await Task.CompletedTask;
        }

        var permissions = context.User.Claims
            .Where(claim => claim.Type == AppClaim.Permission 
            && claim.Value == requirement.Permission 
            && claim.Issuer == "LOCAL AUTHORITY");

        if (permissions.Any())
        {
            context.Succeed(requirement);
            await Task.CompletedTask;
        }
    }
}
