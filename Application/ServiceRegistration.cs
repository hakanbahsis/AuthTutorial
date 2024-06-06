

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        var assembly=Assembly.GetExecutingAssembly();
        return services
            .AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(assembly));
    }
}
