using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class ServiceRegistration
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .AddTransient<AppDbSeeder>();
        return services;
    }
}
