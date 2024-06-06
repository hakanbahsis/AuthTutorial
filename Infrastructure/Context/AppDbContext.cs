using Domain.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;
public class AppDbContext : IdentityDbContext<AppUser,
    AppRole,string,IdentityUserClaim<string>,IdentityUserRole<string>,
    IdentityUserLogin<string>,AppRoleClaim,IdentityUserToken<string>>
{
    public AppDbContext(DbContextOptions options):base(options)
    {
        
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t=>t.GetProperties())
            .Where(p=>p.ClrType == typeof(decimal)||p.ClrType==typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
