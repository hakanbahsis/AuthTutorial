using Application.Features.Identity.Users.Commands;
using Application.Features.Identity.Users.Queries;
using Common.Authorization;
using Common.Requests.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers.Identity;
[Route("api/[controller]")]
public class UsersController : MyBaseController<UsersController>
{
    [HttpPost]
    [MustHavePermission(AppFeature.Users,AppAction.Create)]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistration)
    {
        var response=await MediatorSender
            .Send(new UserRegistrationCommand { UserRegistrationRequest = userRegistration });
        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("{userId}")]
    [MustHavePermission(AppFeature.Users, AppAction.Read)]
    public async Task<IActionResult> GetUserByIdAsync( Guid userId)
    {
        var response=await MediatorSender.Send(new GetUserByIdQuery { UserId = userId });
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpGet]
    [MustHavePermission(AppFeature.Users, AppAction.Read)]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await MediatorSender.Send(new GetAllUsersQuery());
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpPut]
    [MustHavePermission(AppFeature.Users, AppAction.Update)]
    public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserRequest request)
    {
        var response=await MediatorSender.Send(new UpdateUserCommand { UpdateUserRequest = request });
        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

    [HttpPut("change-password")]
    [MustHavePermission(AppFeature.Users, AppAction.Update)]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest request)
    {
        var response=await MediatorSender.Send(new ChangeUserPasswordCommand { ChangePasswordRequest = request });
        if(response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpPut("change-status")]
    [MustHavePermission(AppFeature.Users, AppAction.Update)]
    public async Task<IActionResult> ChangeUserStatus([FromBody] ChangeUserStatusRequest request)
    {
        var response = await MediatorSender.Send(new ChangeUserStatusCommand { ChangeUserStatus = request });
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response) ;
    }

    [HttpGet("roles/{userId}")]
    [MustHavePermission(AppFeature.Users, AppAction.Read)]
    public async Task<IActionResult> GetRoles(Guid userId)
    {
        var response=await MediatorSender.Send(new GetRolesQuery { UserId = userId });
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpPut("user-roles")]
    [MustHavePermission(AppFeature.Users, AppAction.Update)]
    public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesRequest request)
    {
        var response=await MediatorSender.Send(new UpdateUserRolesCommand { UpdateUserRoles=request });
        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

}
