using Application.Features.Identity.Commands;
using Application.Features.Identity.Queries;
using Common.Requests.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity;
[Route("api/[controller]")]
public class UsersController : MyBaseController<UsersController>
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistration)
    {
        var response=await MediatorSender
            .Send(new UserRegistrationCommand { UserRegistrationRequest = userRegistration });
        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserByIdAsync( Guid userId)
    {
        var response=await MediatorSender.Send(new GetUserByIdQuery { UserId = userId });
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await MediatorSender.Send(new GetAllUsersQuery());
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserRequest request)
    {
        var response=await MediatorSender.Send(new UpdateUserCommand { UpdateUserRequest = request });
        if (response.IsSuccessful)
            return Ok(response);
        return BadRequest(response);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest request)
    {
        var response=await MediatorSender.Send(new ChangeUserPasswordCommand { ChangePasswordRequest = request });
        if(response.IsSuccessful)
            return Ok(response);
        return NotFound(response);
    }

    [HttpPut("change-status")]
    public async Task<IActionResult> ChangeUserStatus([FromBody] ChangeUserStatusRequest request)
    {
        var response = await MediatorSender.Send(new ChangeUserStatusCommand { ChangeUserStatus = request });
        if (response.IsSuccessful)
            return Ok(response);
        return NotFound(response) ;
    }

}
