using Application.Features.Identity.Roles.Commands;
using Application.Features.Identity.Roles.Queries;
using Common.Authorization;
using Common.Requests.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers.Identity;
[Route("api/[controller]")]
public class RolesController : MyBaseController<RolesController>
{
    [HttpPost]
    [MustHavePermission(AppFeature.Roles,AppAction.Create)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var response=await MediatorSender.Send(new CreateRoleCommand { RoleRequest = request });
        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

    [HttpGet]
    [MustHavePermission(AppFeature.Roles,AppAction.Read)]
    public async Task<IActionResult> GetAllRoles()
    {
        var response = await MediatorSender.Send(new GetRolesQuery());
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpPut]
    [MustHavePermission(AppFeature.Roles,AppAction.Update)]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
    {
        var response = await MediatorSender.Send(new UpdateRoleCommand { UpdateRoleRequest = request });
        if(response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }
    [HttpGet("{roleId}")]
    [MustHavePermission(AppFeature.Roles,AppAction.Read)]
    public async Task<IActionResult> GetRoleById(string roleId)
    {
        var response=await MediatorSender.Send(new GetRoleByIdQuery { RoleId = roleId });
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpDelete]
    [MustHavePermission(AppFeature.Roles,AppAction.Delete)]
    public async Task<IActionResult> DeleteRole(string RoleId)
    {
        var response=await MediatorSender.Send(new DeleteRoleCommand { RoleId = RoleId });
        if(response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("permissions/{roleId}")]
    [MustHavePermission(AppFeature.RoleClaims,AppAction.Read)]
    public async Task<IActionResult> GetPermissions(string roleId)
    {
        var response=await MediatorSender.Send(new GetPermissionsQuery { RoleId = roleId });
        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

    [HttpPut("update-permissions")]
    [MustHavePermission(AppFeature.RoleClaims,AppAction.Update)]
    public async Task<IActionResult> UpdateRolePermissions([FromBody] UpdateRolePermissionsRequest request)
    {
        var response=await MediatorSender
            .Send(new UpdateRolePermissionsCommand { UpdateRolePermissionsRequest = request });

        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }
}
