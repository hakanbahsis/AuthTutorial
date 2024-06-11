using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Commands;
public class UpdateUserCommand:IRequest<IResponseWrapper>
{
    public UpdateUserRequest     UpdateUserRequest { get; set; }

}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IResponseWrapper>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUserAsync(request.UpdateUserRequest);
    }
}
