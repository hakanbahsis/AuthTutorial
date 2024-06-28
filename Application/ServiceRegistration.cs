

using Application.Pipelines;
using Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        var assembly=Assembly.GetExecutingAssembly();
        services.AddAutoMapper(typeof(ServiceRegistration).Assembly);
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

        return services;
    }
}
