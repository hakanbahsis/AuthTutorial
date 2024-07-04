using Application.Exceptions;
using Common.Responses;
using Common.Responses.Wrappers;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            //Error error = new();
            var responseWrapper = await ResponseWrapper.FailAsync(ex.Message);
            switch (ex)
            {
                case CustomValidationException vex:
                    response.StatusCode=(int)HttpStatusCode.BadRequest;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result=JsonSerializer.Serialize(responseWrapper);
            await response.WriteAsync(result);
        }
    }
}
