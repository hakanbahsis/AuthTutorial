using Microsoft.AspNetCore.Authorization;

namespace WebApi.Permissions;

public class PermissionsRequirement:IAuthorizationRequirement
{
    public string Permission { get; set; }
    public PermissionsRequirement(string permission)
    {
       Permission = permission;
    }
}
