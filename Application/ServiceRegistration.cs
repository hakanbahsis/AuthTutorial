

using Application.Services;
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
        

        return services;
    }
}
