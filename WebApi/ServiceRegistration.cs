using Infrastructure.Context;
using Infrastructure.Models;

namespace WebApi;

public static class ServiceRegistration
{
    internal static  IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        using var serviceScope=app.ApplicationServices.CreateScope();
        var seeders=serviceScope.ServiceProvider.GetServices<AppDbSeeder>();

        foreach (var seeder in seeders)
        {
             seeder.SeedDatabaseAsync().GetAwaiter().GetResult();
        }

        return app;
    }

    internal static IServiceCollection AddIdentitySettings(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}
