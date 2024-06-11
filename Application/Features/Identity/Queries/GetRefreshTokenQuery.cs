using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Queries;
public class GetRefreshTokenQuery:IRequest<IResponseWrapper>
{
    public RefreshTokenRequest  RefreshTokenRequest { get; set; }

}

public class GetRefreshTokenQueryHander : IRequestHandler<GetRefreshTokenQuery, IResponseWrapper>
{
    private readonly ITokenService _tokenService;

    public GetRefreshTokenQueryHander(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<IResponseWrapper> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await _tokenService.GetRefreshTokenAsync(request.RefreshTokenRequest);
    }
}
