using Application.Services.Identity;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.Identity.Queries;
public class GetTokenQuery:IRequest<IResponseWrapper>
{
    public TokenRequest TokenRequest { get; set; }
}

public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, IResponseWrapper>
{
    private readonly ITokenService _tokenService;

    public GetTokenQueryHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<IResponseWrapper> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        return await _tokenService.GetTokenAsync(request.TokenRequest);
    }
}


public class GetTest : IRequest<IResponseWrapper>
{
    public string Test { get; set; }
}


public class GetTestHandler : IRequestHandler<GetTest, IResponseWrapper>
{
    public Task<IResponseWrapper> Handle(GetTest request, CancellationToken cancellationToken)
    {
        return ResponseWrapper.SuccessAsync("Ok");
    }
}

